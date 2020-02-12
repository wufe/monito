package process

import (
	"fmt"
	"sync"

	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
)

type FetchProcess struct {
	db                 *gorm.DB
	request            *models.Request
	options            *models.RequestOptions
	stopChan           chan struct{}
	retrievalDoneChan  chan struct{}
	linksChan          chan *models.Link
	performLinkRequest func() <-chan struct{}
}

func NewFetchProcess(
	db *gorm.DB,
	request *models.Request,
	options *models.RequestOptions,
	stopChan chan struct{},
	retrievalDoneChan chan struct{},
	linksChan chan *models.Link,
	performLinkRequest func() <-chan struct{},
) *FetchProcess {
	return &FetchProcess{
		db,
		request,
		options,
		stopChan,
		retrievalDoneChan,
		linksChan,
		performLinkRequest,
	}
}

func (process *FetchProcess) Start() chan struct{} {
	doneChan := make(chan struct{}, 1)

	requestQueueChannel := make(chan struct{}, process.options.Threads)
	for i := 0; i < process.options.Threads; i++ {
		requestQueueChannel <- struct{}{}
	}

	var wg sync.WaitGroup

	go func() {
	L:
		for {
			select {
			case <-requestQueueChannel:
				go func() {
					wg.Add(1)
					<-process.performLinkRequest()
					requestQueueChannel <- struct{}{}
					wg.Done()
				}()
			case <-process.stopChan:
				process.stopChan <- struct{}{}
				fmt.Println("Fetch process aborted")
				break L
			// Should enter here only if request queue channel is empty and done
			case <-process.retrievalDoneChan:
				process.retrievalDoneChan <- struct{}{}
				if len(process.linksChan) == 0 {
					fmt.Println("#004 Fetch process cannot fetch more links..")
					fmt.Println("#004 Stopping fetch process..")

					fmt.Println("#004 Done")
					break L
				}
			default:
			}
		}

		wg.Wait()
		doneChan <- struct{}{}
		fmt.Println(fmt.Sprintf("Process ended for request #%d", process.request.ID))
	}()

	return doneChan
}
