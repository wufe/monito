package process

import (
	"fmt"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
)

type StatusCheckProcess struct {
	db                *gorm.DB
	request           *models.Request
	retrievalDoneChan chan struct{}
	fetchDoneChan     chan struct{}
}

func NewStatusCheckProcess(db *gorm.DB, request *models.Request, retrievalDoneChan chan struct{}, fetchDoneChan chan struct{}) *StatusCheckProcess {
	return &StatusCheckProcess{
		db,
		request,
		retrievalDoneChan,
		fetchDoneChan,
	}
}

func (process *StatusCheckProcess) Start() chan struct{} {
	doneChan := make(chan struct{}, 1)
	go func() {
	L:
		for {
			select {
			case <-process.retrievalDoneChan:
				fmt.Println("#001 Status check process ending because there are no more links to be requested..")
				process.retrievalDoneChan <- struct{}{}
				fmt.Println("#001 Done.")
				break L
			case <-process.fetchDoneChan:
				fmt.Println("#002 Status check process ending because the fetch process has done..")
				process.fetchDoneChan <- struct{}{}
				fmt.Println("#002 Done.")
				break L
			default:
				status := process.getJobStatus()
				if status == models.RequestStatusAbort {
					fmt.Println("#003 Status check process ending because abortion has been requested..")
					// process.stopChan <- struct{}{}
					doneChan <- struct{}{}
					fmt.Println("#003 Done.")
					break L
				}
				time.Sleep(500 * time.Millisecond)
			}
		}
	}()
	return doneChan
}

func (process *StatusCheckProcess) getJobStatus() models.RequestStatus {
	foundRequest := models.Request{}

	process.db.Model(&models.Request{}).
		Where(&models.Request{ID: process.request.ID}).
		First(&foundRequest)

	process.request.Status = foundRequest.Status

	return foundRequest.Status
}
