package process

import (
	"fmt"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
)

type LinkRetrievalProcess struct {
	db       *gorm.DB
	request  *models.Request
	options  *models.RequestOptions
	stopChan chan struct{}
}

func NewLinkRetrievalProcess(db *gorm.DB, request *models.Request, options *models.RequestOptions, stopChan chan struct{}) *LinkRetrievalProcess {
	return &LinkRetrievalProcess{
		db,
		request,
		options,
		stopChan,
	}
}

func (process *LinkRetrievalProcess) Start() (links chan *models.Link, done chan struct{}) {
	doneChan := make(chan struct{}, 1)
	linksChan := make(chan *models.Link)

	go func() {
		var lastLinkID uint
	L:
		for {
			select {
			case <-process.stopChan:
				process.stopChan <- struct{}{}
				fmt.Println("Retrieval process aborted.")
				doneChan <- struct{}{}
				break L
			default:
				link := process.getLink(lastLinkID)
				if link.ID > 0 {
					lastLinkID = link.ID
					linksChan <- link
				} else {
					doneChan <- struct{}{}
					break L
				}
			}
		}

		process.request.Status = models.RequestStatusDone

		process.db.Model(&models.Request{}).
			Update(process.request)

		fmt.Println("Retrieval queue process ended.")
	}()

	return linksChan, doneChan

}

func (process *LinkRetrievalProcess) getLink(greaterThanID uint) *models.Link {

	foundLink := models.Link{}

	process.db.Model(&models.Link{}).
		Where("request_id = ? AND status = ? AND id > ?", process.request.ID, models.LinkStatusIdle, greaterThanID).
		First(&foundLink)

	if foundLink.ID > 0 {
		process.db.Model(&foundLink).
			Where(&models.Link{ID: foundLink.ID}).
			Update(map[string]interface{}{"status": models.LinkStatusAcknowledged, "updated_at": time.Now()})
	}

	return &foundLink
}
