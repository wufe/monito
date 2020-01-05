package models

import "time"

type Worker struct {
	ID        uint `gorm:"primary_key"`
	Hostname  string
	UUID      string `gorm:"type:varchar(100);unique_index"`
	Ping      time.Time
	CreatedAt time.Time
	UpdatedAt time.Time
	Queues    []Queue
}
