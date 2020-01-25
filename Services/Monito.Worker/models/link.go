package models

import "time"

const (
	LinkStatusIdle         LinkStatus = 0
	LinkStatusAcknowledged LinkStatus = 1
	LinkStatusInProgress   LinkStatus = 2
	LinkStatusDone         LinkStatus = 3
)

type LinkStatus uint8

type Link struct {
	ID                  uint `gorm:"primary_key"`
	URL                 string
	Status              LinkStatus
	Output              string
	StatusCode          int
	AdditionalData      string
	UUID                string `gorm:"type:varchar(100);unique_index"`
	CreatedAt           time.Time
	UpdatedAt           time.Time
	RequestID           uint
	RedirectsFromLinkID *uint `gorm:"column:redirects_from_link_id"`
}
