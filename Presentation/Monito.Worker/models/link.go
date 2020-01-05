package models

import "time"

const (
	LinkStatusIdle       = 0
	LinkStatusInProgress = 1
	LinkStatusDone       = 2
)

type LinkStatus uint8

type Link struct {
	ID              uint `gorm:"primary_key"`
	URL             string
	Status          LinkStatus
	StatusCode      int
	AdditionalData  string
	CreatedAt       time.Time
	UpdatedAt       time.Time
	RedirectionLink *Link
	Request         Request
}
