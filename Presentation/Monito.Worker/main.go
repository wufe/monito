package main

import (
	"log"
	"os"
	"sync"

	_ "github.com/jinzhu/gorm/dialects/mysql"
	"github.com/joho/godotenv"
	"github.com/wufe/monito/worker/cli"
	"github.com/wufe/monito/worker/database"
	"github.com/wufe/monito/worker/services"
)

func main() {

	parsedCliArguments := cli.ParseCliArguments(os.Args)

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	db := database.ConnectToDatabase()
	defer db.Close()

	var waitGroup sync.WaitGroup

	waitGroup.Add(1)
	queues := services.RegisterWorker(db, parsedCliArguments)
	services.NewQueueOrchestrator(queues, db).StartQueues()

	waitGroup.Wait()
}
