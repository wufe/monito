package services

import (
	"fmt"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
)

type QueueOrchestrator struct {
	queues            []*models.Queue
	db                *gorm.DB
	jobsByRequestType *map[models.RequestType]chan *models.Request
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
	orchestrator.buildJobChans()
	orchestrator.startWorkers()
	orchestrator.startRetrievalProcess()

	return orchestrator
}

// I supposedly have one orchestrator at a time,
// so if there are requests with a pending status
// we might need to reset them.
func (orchestrator *QueueOrchestrator) resetRequests() {
	orchestrator.db.Model(&models.Request{}).
		Where(&models.Request{Status: models.RequestStatusAcknowledged}).
		Update(map[string]interface{}{"status": models.RequestStatusReady, "updated_at": time.Now()})
}

func (orchestrator *QueueOrchestrator) buildJobChans() {
	jobsByRequestType := make(map[models.RequestType]chan *models.Request)
	jobsByRequestType[models.RequestTypeBatch] = make(chan *models.Request)
	jobsByRequestType[models.RequestTypeSimple] = make(chan *models.Request)

	orchestrator.jobsByRequestType = &jobsByRequestType
}

func (orchestrator *QueueOrchestrator) startWorkers() {
	for _, queue := range orchestrator.queues {

		workingQueue := NewWorkingQueue(nil, queue, orchestrator.db)
		requestType := workingQueue.GetRequestTypeByQueue()

		go func(workingQueue *WorkingQueue, jobs <-chan *models.Request) {
			for request := range jobs {
				workingQueue.Work(request)
			}
		}(workingQueue, (*orchestrator.jobsByRequestType)[requestType])

		fmt.Println(fmt.Sprintf("Started queue with request type %s", models.GetRequestTypeLabel(requestType)))

	}
}

func (orchestrator *QueueOrchestrator) startRetrievalProcess() {
	go func(jobsByRequestType map[models.RequestType]chan *models.Request) {
		defer close(jobsByRequestType[models.RequestTypeBatch])
		defer close(jobsByRequestType[models.RequestTypeSimple])
		for {
			fmt.Println(fmt.Sprintf("[Retrieval] Looking for a request.."))
			request := orchestrator.findRequest()
			if request.ID == 0 {
				fmt.Println(fmt.Sprintf("[Retrieval] No request found"))
				time.Sleep(time.Millisecond * 2000)
			} else {
				fmt.Println(fmt.Sprintf("[Retrieval] Request found!"))
				jobsByRequestType[request.Type] <- request
				fmt.Println(fmt.Sprintf("[Retrieval] Request enqueued as job (%s)", models.GetRequestTypeLabel(request.Type)))
			}
		}
	}(*orchestrator.jobsByRequestType)
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
