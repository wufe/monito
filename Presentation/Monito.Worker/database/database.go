package database

import (
	"fmt"
	"os"

	"github.com/jinzhu/gorm"
)

func ConnectToDatabase() *gorm.DB {
	user := os.Getenv("MYSQL_USER")
	password := os.Getenv("MYSQL_PASSWORD")
	host := os.Getenv("MYSQL_HOST")
	database := os.Getenv("MYSQL_DATABASE")
	port := os.Getenv("MYSQL_PORT")
	connectionString := fmt.Sprintf("%s:%s@(%s:%s)/%s?charset=utf8&parseTime=True&loc=Local", user, password, host, port, database)

	db, err := gorm.Open("mysql", connectionString)
	if err != nil {
		panic(err)
	}

	return db
}
