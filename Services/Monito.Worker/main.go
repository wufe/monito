package main

import (
	"fmt"
	"log"
	"os"
	"runtime"
	"sync"
	"time"

	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/mysql"
	"github.com/joho/godotenv"
	"github.com/wufe/monito/worker/cli"
	"github.com/wufe/monito/worker/database"
	"github.com/wufe/monito/worker/models"
	"github.com/wufe/monito/worker/services"
)

func main() {

	runtime.GOMAXPROCS(runtime.NumCPU())

	processInterrupt := make(chan struct{})

	parsedCliArguments := cli.ParseCliArguments(os.Args)

	err := godotenv.Load()
	if err != nil {
		fmt.Println(err)
		log.Fatal("Error loading .env file")
	}

	db := database.ConnectToDatabase()
	defer db.Close()

	var waitGroup sync.WaitGroup

	waitGroup.Add(1)

	queues := services.RegisterWorker(db, parsedCliArguments)
	fmt.Println("Worker registered itself.")

	// Resetting requests status
	db.Model(&models.Request{}).
		Where(&models.Request{Status: models.RequestStatusAcknowledged}).
		Update(map[string]interface{}{"status": models.RequestStatusReady, "updated_at": time.Now()})

	// Jobs channel
	jobsByRequestType := make(map[models.RequestType]chan *models.Request)
	jobsByRequestType[models.RequestTypeBatch] = make(chan *models.Request)
	jobsByRequestType[models.RequestTypeSimple] = make(chan *models.Request)
	// jobs := make(chan *models.Request)

	// Build and start workers
	startWorkers(db, queues, jobsByRequestType)

	// Retrieval process
	go func(jobsByRequestType map[models.RequestType]chan *models.Request) {
		defer close(jobsByRequestType[models.RequestTypeBatch])
		defer close(jobsByRequestType[models.RequestTypeSimple])
		for {
			select {
			case <-processInterrupt:
				return
			default:
				fmt.Println(fmt.Sprintf("[Retrieval] Looking for a request.."))
				request := findRequest(db)
				if request.ID == 0 {
					fmt.Println(fmt.Sprintf("[Retrieval] No request found"))
					time.Sleep(time.Millisecond * 2000)
				} else {
					fmt.Println(fmt.Sprintf("[Retrieval] Request found!"))
					jobsByRequestType[request.Type] <- request
					fmt.Println(fmt.Sprintf("[Retrieval] Request enqueued as job (%s)", models.GetRequestTypeLabel(request.Type)))
				}
			}
		}
	}(jobsByRequestType)

	// orchestrator := services.NewQueueOrchestrator(queues, db).Init()

	// fmt.Println("Orchestrator initialized.")

	// orchestrator.StartQueues()

	// fmt.Println("Worker started.")

	// // sigs := make(chan os.Signal)
	// // signal.Notify(sigs, syscall.SIGINT)
	// // go func() {

	// // 	<-sigs
	// // 	fmt.Println("OK")
	// // 	processInterrupt <- struct{}{}
	// // 	waitGroup.Done()
	// // }()

	waitGroup.Wait()

}

func findRequest(db *gorm.DB) *models.Request {
	foundRequest := models.Request{}

	db.Model(&models.Request{}).
		Where(&models.Request{Status: models.RequestStatusReady}).
		First(&foundRequest)

	if foundRequest.ID > 0 {

		db.Model(&foundRequest).
			Where(&models.Request{ID: foundRequest.ID}).
			Update(map[string]interface{}{"status": models.RequestStatusAcknowledged, "updated_at": time.Now()})
	}

	return &foundRequest
}

func startWorkers(db *gorm.DB, queues []*models.Queue, jobsByRequestType map[models.RequestType]chan *models.Request) {
	for _, queue := range queues {

		workingQueue := services.NewWorkingQueue(nil, queue, db)
		requestType := workingQueue.GetRequestTypeByQueue()

		go func(workingQueue *services.WorkingQueue, jobs <-chan *models.Request) {
			for request := range jobs {
				workingQueue.Work(request)
			}
		}(workingQueue, jobsByRequestType[requestType])

		fmt.Println(fmt.Sprintf("Started queue with request type %s", models.GetRequestTypeLabel(requestType)))

	}
}
