package services

import (
	"fmt"
	"net/http"
	"strings"
	"time"

	"github.com/google/uuid"
	"github.com/jinzhu/gorm"
	"github.com/wufe/monito/worker/models"
)

// HTTPServiceRequest - Used to perform and manage a request to a link
type HTTPServiceRequest struct {
	job     *JobProcess
	request *models.Request
	options *models.RequestOptions
	link    *models.Link
	db      *gorm.DB
}

// NewHTTPRequestService - Creates an HttpServiceRequest for a given link
func NewHTTPRequestService(link *models.Link, job *JobProcess) *HTTPServiceRequest {
	return &HTTPServiceRequest{
		job:     job,
		request: job.request,
		options: job.options,
		link:    link,
		db:      job.db,
	}
}

func (httpService *HTTPServiceRequest) Send() <-chan struct{} {
	done := make(chan struct{})
	go func() {
		httpService.setInProgress()
		httpService.sendRequest()
		done <- struct{}{}
	}()
	return done
}

func (httpService *HTTPServiceRequest) setInProgress() {
	httpService.db.Model(&models.Link{}).
		Where(&models.Link{ID: httpService.link.ID}).
		Updates(map[string]interface{}{"status": models.LinkStatusInProgress})
}

func (httpService *HTTPServiceRequest) sendRequest() {

	var httpMethod string

	switch httpService.options.Method {
	case models.RequestOptionsMethodTypeHead:
		httpMethod = "HEAD"
	default:
		httpMethod = "GET"
	}

	var URL = httpService.link.URL
	if !strings.HasPrefix(URL, "http") {
		URL = "http://" + URL
	}

	requestLinkedListHead := &HTTPRequestLinkedList{
		URL:             URL,
		Method:          httpMethod,
		UserAgent:       httpService.options.UserAgent,
		MaxRedirects:    httpService.options.Redirects,
		Timeout:         httpService.options.Timeout,
		CurrentRedirect: 0,
	}

	fmt.Println("Starting request")

	requestLinkedListHead = NewHTTPRequest(requestLinkedListHead)
	httpService.parseAndSaveLink(requestLinkedListHead)

}

func (httpService *HTTPServiceRequest) parseAndSaveLink(linkedList *HTTPRequestLinkedList) uint {
	var linkID uint
	if linkedList.RedirectsFrom != nil {
		linkID = httpService.parseAndSaveLink(linkedList.RedirectsFrom)

		redirectionLink := &models.Link{
			RequestID: httpService.link.RequestID,
			URL:       linkedList.URL,
			Status:    models.LinkStatusDone,

			UUID:                uuid.New().String(),
			CreatedAt:           time.Now(),
			UpdatedAt:           time.Now(),
			RedirectsFromLinkID: &linkID,
		}

		if linkedList.ResponseError != nil {
			redirectionLink.Output = string(linkedList.ResponseError.Error())
		} else {
			redirectionLink.Output = linkedList.Response.Status
			redirectionLink.StatusCode = linkedList.Response.StatusCode
		}

		httpService.db.Model(&models.Link{}).
			Create(redirectionLink)
	} else {

		link := httpService.link
		link.Status = models.LinkStatusDone

		if linkedList.ResponseError != nil {
			link.Output = string(linkedList.ResponseError.Error())
		} else {
			link.Output = linkedList.Response.Status
			link.StatusCode = linkedList.Response.StatusCode
		}

		link.UpdatedAt = time.Now()

		httpService.db.Model(&models.Link{}).
			Update(link)

		linkID = httpService.link.ID
	}
	return linkID
}

type HTTPRequestLinkedList struct {
	URL             string
	Method          string
	UserAgent       string
	Response        *http.Response
	ResponseError   error
	MaxRedirects    int
	CurrentRedirect int
	Timeout         int
	RedirectsFrom   *HTTPRequestLinkedList
}

func NewHTTPRequest(stackItem *HTTPRequestLinkedList) *HTTPRequestLinkedList {

	fmt.Println(fmt.Sprintf("Requesting [%s]", stackItem.URL))

	var response *http.Response
	var responseError error

	client := &http.Client{
		CheckRedirect: func(req *http.Request, via []*http.Request) error {
			return http.ErrUseLastResponse
		},
		Timeout: time.Duration(stackItem.Timeout) * time.Millisecond,
	}

	request, _ := http.NewRequest(stackItem.Method, stackItem.URL, nil)

	request.Header.Set("User-Agent", stackItem.UserAgent)

	response, responseError = client.Do(request)

	if responseError != nil {
		stackItem.ResponseError = responseError
		return stackItem
	} else {
		stackItem.Response = response
		if response.StatusCode == 301 || response.StatusCode == 302 || response.StatusCode == 307 || response.StatusCode == 308 {
			var followRedirect = stackItem.MaxRedirects > stackItem.CurrentRedirect
			if followRedirect {
				fmt.Println(fmt.Sprintf("Following redirect [%s] -> [%s]", stackItem.URL, response.Header.Get("Location")))
				followStackItem := &HTTPRequestLinkedList{
					// TODO: Check if location is not empty
					URL:             response.Header.Get("Location"),
					Method:          stackItem.Method,
					Response:        nil,
					ResponseError:   nil,
					Timeout:         stackItem.Timeout,
					UserAgent:       stackItem.UserAgent,
					MaxRedirects:    stackItem.MaxRedirects,
					CurrentRedirect: stackItem.CurrentRedirect + 1,
					RedirectsFrom:   stackItem,
				}
				return NewHTTPRequest(followStackItem)
			} else {
				return stackItem
			}
		} else {
			return stackItem
		}

	}

}
