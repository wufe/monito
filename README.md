# Mònito

Mònito is a link status checker.

Provides a web UI for submission with a queue system on the background.

[![Mònito][image]][hyperlink]

  [hyperlink]: https://monito.bembi.dev
  [image]: https://raw.githubusercontent.com/wufe/monito/master/.img/compressed-wide.jpg (Mònito)

## How to

All automation scripts are in the `Scripts` folders.  
Require `powershell`.

- **Start** - `pwsh ./Scripts/Deploy.ps1`
- **Stop** - `pwsh ./Scripts/Stop.ps1`
- **Read logs** - `pwsh ./Scripts/Logs.ps1`

### Implementation concepts

The underlying database is MySQL.  
The web UI is written using asp.net core 3 and performs polling on the database for updates.  
The frontend of the UI uses React, its hooks, Redux and Thunks.  
The worker is implemented in Go.

The whole system is containerized with Docker.

### What's ahead

Future implementations may include:  
- Authentication and authorization
- Workers orchestration among different processes (micro-services like structure), with gRPC
- JSON asset download with -maybe- custom *download recipes*
- Back-off delay for the same host (with some kind of log)
- Use of secret management for the whole infrastructure