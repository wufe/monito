package services

import (
	"encoding/json"
	"fmt"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
	"github.com/wufe/monito/worker/services/process"
	"github.com/wufe/monito/worker/utils"
)

type JobProcess struct {
	request           *models.Request
	options           *models.RequestOptions
	db                *gorm.DB
	retrievalDoneChan chan struct{}
	fetchDoneChan     chan struct{}
	stopChan          chan struct{}
	linksChan         chan *models.Link
}

func NewJob(request *models.Request, db *gorm.DB) *JobProcess {
	return &JobProcess{
		request: request,
		db:      db,
	}
}

func (job *JobProcess) Start() {
	job.resetLinks()
	job.loadOptions()

	job.stopChan = make(chan struct{}, 1)
	job.fetchDoneChan = make(chan struct{}, 1)
	job.linksChan = make(chan *models.Link, 1)
	job.retrievalDoneChan = make(chan struct{}, 1)

	job.startStatusCheckProcess()
	job.startLinkRetrievalProcess()
	job.startFetch()

	job.cleanup()
}

// Recover from inconsistent state
func (job *JobProcess) resetLinks() {
	job.db.Model(&models.Link{}).
		Where(&models.Link{RequestID: job.request.ID, Status: models.LinkStatusAcknowledged}).
		Update(map[string]interface{}{"status": models.LinkStatusIdle, "updated_at": time.Now()})
}

func (job *JobProcess) loadOptions() {

	requestOptions := models.RequestOptions{
		Method:    models.RequestOptionsMethodTypeHead,
		Redirects: 5,
		Threads:   2,
		Timeout:   10000,
		UserAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36",
	}
	err := json.Unmarshal([]byte(job.request.Options), &requestOptions)
	if err != nil {
		fmt.Println("Could not deserialize request options: going with default values.")
	}
	job.options = &requestOptions
}

func (job *JobProcess) startStatusCheckProcess() {
	doneChan := process.
		NewStatusCheckProcess(job.db, job.request, job.retrievalDoneChan, job.fetchDoneChan).
		Start()

	go func() {
		<-doneChan
		job.stopChan <- struct{}{}

		emptyAndCloseLinksChan(job.linksChan)

		close(doneChan)
	}()
}

func (job *JobProcess) startLinkRetrievalProcess() {
	linksChan, doneChan := process.
		NewLinkRetrievalProcess(job.db, job.request, job.options, job.stopChan).
		Start()

	job.linksChan = linksChan

	go func() {
	L:
		for {
			select {
			case <-doneChan:
				close(doneChan)
				job.retrievalDoneChan <- struct{}{}
				break L
			default:
			}
		}
	}()
}

func (job *JobProcess) startFetch() {
	doneChan := process.
		NewFetchProcess(job.db, job.request, job.options, job.stopChan, job.retrievalDoneChan, job.linksChan, job.performLinkRequest).
		Start()

	<-doneChan
	close(doneChan)
	job.fetchDoneChan <- struct{}{}
}

func (job *JobProcess) performLinkRequest() <-chan struct{} {
	doneChan := make(chan struct{}, 1)
	go func() {
	L:
		for {
			select {
			case <-job.retrievalDoneChan:
				job.retrievalDoneChan <- struct{}{}
				if len(job.linksChan) == 0 {
					doneChan <- struct{}{}
					break L
				}

			case link := <-job.linksChan:
				<-NewHTTPRequestService(link, job).Send()
				utils.SetTimeout(func() {
					doneChan <- struct{}{}
				}, 1000)
				break L
			}
		}
	}()
	return doneChan
}

func (job *JobProcess) cleanup() {
	close(job.stopChan)
	close(job.retrievalDoneChan)
	close(job.fetchDoneChan)
	emptyAndCloseLinksChan(job.linksChan)
}

// When a job is aborted, the links channel might not be empty
// and a go routine is waiting the opportunity to enqueue an item.
// So we first try to free its queue and then close it.
func emptyAndCloseLinksChan(c chan *models.Link) {
	select {
	case _, ok := <-c:
		if ok {
			close(c)
		}
	default:
	}
}
