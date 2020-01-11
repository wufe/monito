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

	// request := services.NewHTTPRequest(&services.HTTPRequestStackItem{
	// 	Url:             "http://bembi.dev",
	// 	Method:          "GET",
	// 	MaxRedirects:    1,
	// 	CurrentRedirect: 0,
	// })

	// fmt.Println(*request)
	// fmt.Println(request.Response.StatusCode)
	// fmt.Println(request.Response.Request.URL)
	// fmt.Println(request.RedirectsFrom.Response.Request.URL)
	// panic(false)

	parsedCliArguments := cli.ParseCliArguments(os.Args)

	err := godotenv.Load()
	if err != nil {
		fmt.Println(error.Error())
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
