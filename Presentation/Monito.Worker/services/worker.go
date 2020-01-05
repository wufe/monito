package services

import (
	"os"
	"time"

	"github.com/google/uuid"
	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/cli"
	"github.com/wufe/monito/worker/models"
	"github.com/wufe/monito/worker/utils"
)

func RegisterWorker(db *gorm.DB, cliArgs *cli.ParsedCliArguments) []*models.Queue {
	hostname, err := os.Hostname()
	if err != nil {
		panic(err)
	}

	worker := models.Worker{
		Hostname:  hostname,
		Ping:      time.Now(),
		CreatedAt: time.Now(),
		UpdatedAt: time.Now(),
		UUID:      uuid.New().String(),
	}

	db.Create(&worker)

	workerId := worker.ID

	go func() {
		utils.SetInterval(func() {

			worker := models.Worker{}
			db.First(&worker, workerId)

			worker.Ping = time.Now()
			worker.UpdatedAt = time.Now()
			db.Model(&worker).Update(&worker)
		}, 5000, false)
	}()

	queues := []*models.Queue{}

	simpleQueues := cliArgs.Simple
	for i := 0; i < simpleQueues; i++ {
		queues = append(queues, &models.Queue{
			WorkerID:  workerId,
			Type:      models.QueueTypeSimple,
			Status:    models.QueueStatusIdle,
			UUID:      uuid.New().String(),
			CreatedAt: time.Now(),
			UpdatedAt: time.Now(),
		})
	}

	basicQueues := cliArgs.Basic
	for i := 0; i < basicQueues; i++ {
		queues = append(queues, &models.Queue{
			WorkerID:  workerId,
			Type:      models.QueueTypeBasic,
			Status:    models.QueueStatusIdle,
			UUID:      uuid.New().String(),
			CreatedAt: time.Now(),
			UpdatedAt: time.Now(),
		})
	}

	priorityQueues := cliArgs.Priority
	for i := 0; i < priorityQueues; i++ {
		queues = append(queues, &models.Queue{
			WorkerID:  workerId,
			Type:      models.QueueTypePriority,
			Status:    models.QueueStatusIdle,
			UUID:      uuid.New().String(),
			CreatedAt: time.Now(),
			UpdatedAt: time.Now(),
		})
	}

	for _, queue := range queues {
		db.Create(queue)
	}

	return queues
}
