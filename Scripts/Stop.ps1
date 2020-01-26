[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $Development
)

if ($Development) {
    docker-compose down
} else {
    docker-compose -f ./docker-compose.yml -f ./docker-compose.prod.yml down
}
