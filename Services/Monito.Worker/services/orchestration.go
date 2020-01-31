package services

import (
	"time"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
	"github.com/wufe/monito/worker/utils"
)

type QueueOrchestrator struct {
	queues               []*models.Queue
	db                   *gorm.DB
	dequeueChan          map[models.RequestType]chan *requestRetrievalQueueItem
	availableRequestChan map[models.RequestType]chan *models.Request
}

func NewQueueOrchestrator(queues []*models.Queue, db *gorm.DB) *QueueOrchestrator {
	orchestrator := &QueueOrchestrator{
		queues: queues,
		db:     db,
	}
	return orchestrator
}

func (orchestrator *QueueOrchestrator) Init() *QueueOrchestrator {
	orchestrator.resetRequests()
	orchestrator.startRetrievalProcess()
	orchestrator.startDequeueProcess()
	return orchestrator
}

func (orchestrator *QueueOrchestrator) StartQueues() {
	go func() {
		for _, queue := range orchestrator.queues {
			NewWorkingQueue(orchestrator, queue, orchestrator.db).Start()
		}
	}()
}

func (orchestrator *QueueOrchestrator) GetRequest(requestType models.RequestType) chan *models.Request {
	return orchestrator.enqueueRequestRetrieval(requestType)
}

// I supposedly have one orchestrator at a time,
// so if there are requests with a pending status
// we might need to reset them.
func (orchestrator *QueueOrchestrator) resetRequests() {
	orchestrator.db.Model(&models.Request{}).
		Where(&models.Request{Status: models.RequestStatusAcknowledged}).
		Update(map[string]interface{}{"status": models.RequestStatusReady, "updated_at": time.Now()})
}

func (orchestrator *QueueOrchestrator) startDequeueProcess() {
	dequeueChan := make(map[models.RequestType]chan *requestRetrievalQueueItem)
	dequeueChan[models.RequestTypeSimple] = make(chan *requestRetrievalQueueItem, 1)
	dequeueChan[models.RequestTypeBatch] = make(chan *requestRetrievalQueueItem, 1)
	orchestrator.dequeueChan = dequeueChan

	for k := range orchestrator.dequeueChan {
		requestType := k
		go func() {
			for {
				queueItem := <-orchestrator.dequeueChan[requestType]

				// fmt.Println(fmt.Sprintf("Waiting for a request of type %s to be found", models.GetRequestTypeLabel(requestType)))
				foundRequest := <-orchestrator.availableRequestChan[requestType]
				// fmt.Println(fmt.Sprintf("Request of type %s found.", models.GetRequestTypeLabel(requestType)))

				// Return to the caller the request found
				queueItem.returnChan <- foundRequest
			}
		}()
	}
}

func (orchestrator *QueueOrchestrator) startRetrievalProcess() {
	availableRequestChanDictionary := make(map[models.RequestType]chan *models.Request)
	availableRequestChanDictionary[models.RequestTypeBatch] = make(chan *models.Request, 1)
	availableRequestChanDictionary[models.RequestTypeSimple] = make(chan *models.Request, 1)
	orchestrator.availableRequestChan = availableRequestChanDictionary

	utils.SetInterval(orchestrator.retrievalProcess, 2000, false)
}

func (orchestrator *QueueOrchestrator) retrievalProcess() {

	request := orchestrator.findRequest()
	if request.ID == 0 {
		// fmt.Println("Not found")
	} else {

		orchestrator.availableRequestChan[request.Type] <- request
		// fmt.Println(fmt.Sprintf("Found (%s)!", models.GetRequestTypeLabel(request.Type)))
	}

	// fmt.Println("Orchestrator 5s timeout")
}

type requestRetrievalQueueItem struct {
	requestType models.RequestType
	returnChan  chan *models.Request
}

func (orchestrator *QueueOrchestrator) enqueueRequestRetrieval(requestType models.RequestType) chan *models.Request {
	returnChan := make(chan *models.Request)
	// Building a Go func in order to do not wait
	// for the request to be enqueued.
	// Might use buffered channel but since the traffic is not known,
	// the buffer size is also not known.
	go func() {
		orchestrator.dequeueChan[requestType] <- &requestRetrievalQueueItem{
			requestType: requestType,
			returnChan:  returnChan,
		}
	}()
	return returnChan
}

func (orchestrator *QueueOrchestrator) findRequest() *models.Request {
	foundRequest := models.Request{}

	orchestrator.db.Model(&models.Request{}).
		Where(&models.Request{Status: models.RequestStatusReady}).
		First(&foundRequest)

	if foundRequest.ID > 0 {

		orchestrator.db.Model(&foundRequest).
			Where(&models.Request{ID: foundRequest.ID}).
			Update(map[string]interface{}{"status": models.RequestStatusAcknowledged, "updated_at": time.Now()})
	}

	return &foundRequest
}
