package cli

import (
	"strconv"

	"github.com/wufe/monito/worker/utils"
)

type ParsedCliArguments struct {
	Simple   int
	Basic    int
	Priority int
}

func ParseCliArguments(args []string) *ParsedCliArguments {
	queueTypeArgs := []string{"--simple", "--basic", "--priority"}
	queueTypeDictionary := make(map[string]int)
	for _, queueTypeArg := range queueTypeArgs {
		index := utils.SliceIndex(len(args), func(i int) bool { return args[i] == queueTypeArg })
		if index > -1 {
			if (index + 1) < len(args) {
				instances, err := strconv.Atoi(args[index+1])
				if err != nil {
					instances = 1
				}
				queueTypeDictionary[queueTypeArg] = instances
				// try parse int
			} else {
				queueTypeDictionary[queueTypeArg] = 1
			}
		} else {
			queueTypeDictionary[queueTypeArg] = 0
		}
	}

	parsedArguments := &ParsedCliArguments{}

	if val, ok := queueTypeDictionary[queueTypeArgs[0]]; ok {
		parsedArguments.Simple = val
	}

	if val, ok := queueTypeDictionary[queueTypeArgs[1]]; ok {
		parsedArguments.Basic = val
	}

	if val, ok := queueTypeDictionary[queueTypeArgs[2]]; ok {
		parsedArguments.Priority = val
	}

	return parsedArguments
}
