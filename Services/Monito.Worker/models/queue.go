package models

import (
	"time"
)

const (
	QueueStatusIdle    QueueStatus = 0
	QueueStatusBusy    QueueStatus = 1
	QueueStatusOffline QueueStatus = 2

	QueueTypeBasic    QueueType = 1
	QueueTypePriority QueueType = 2
	QueueTypeSimple   QueueType = 3
)

type QueueStatus uint8
type QueueType uint8

type Queue struct {
	ID        uint        `gorm:"primary_key"`
	Type      QueueType   `gorm:"type:tinyint"`
	Status    QueueStatus `gorm:"type:tinyint"`
	UUID      string      `gorm:"type:varchar(100);unique_index"`
	CreatedAt time.Time
	UpdatedAt time.Time
	WorkerID  uint
}
