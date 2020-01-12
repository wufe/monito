package models

import (
	"time"
)

const (
	// RequestStatusIncomplete - The request has not been completed yet (i.e. the links have not been created yet)
	RequestStatusIncomplete RequestStatus = 0
	// RequestStatusReady - The request has ben fulfilled
	RequestStatusReady RequestStatus = 1
	// RequestStatusAcknowledged - The request has been found by the orchestrator (i.e. locked)
	RequestStatusAcknowledged RequestStatus = 2
	// RequestStatusInProgress - The request is being processed by a Queue
	RequestStatusInProgress RequestStatus = 3
	// RequestStatusDone - The request processing ended
	RequestStatusDone RequestStatus = 4

	RequestTypeSimple RequestType = 1
	RequestTypeBatch  RequestType = 2

	RequestOptionsMethodTypeGet  RequestOptionsMethodType = 1
	RequestOptionsMethodTypeHead RequestOptionsMethodType = 2
)

type RequestStatus uint8

type RequestType uint8

type Request struct {
	ID        uint          `gorm:"primary_key"`
	IP        string        `gorm:"type:varchar(100)"`
	Type      RequestType   `gorm:"type:tinyint"`
	Options   string        `gorm:"type:text"`
	Status    RequestStatus `gorm:"type:tinyint"`
	UUID      string        `gorm:"type:varchar(100);unique_index"`
	CreatedAt time.Time
	UpdatedAt time.Time
	Links     []Link
}

type RequestOptionsMethodType uint8

type RequestOptions struct {
	Method    RequestOptionsMethodType
	Redirects int
	Threads   int
	Timeout   int
	UserAgent string
}

func GetRequestTypeLabel(requestType RequestType) string {
	var label string
	switch requestType {
	case RequestTypeBatch:
		label = "batch"
	case RequestTypeSimple:
		label = "simple"
	}
	return label
}
