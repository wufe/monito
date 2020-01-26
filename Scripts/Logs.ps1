[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $Development
)

if ($Development) {
    docker-compose logs -f
} else {
    docker-compose -f ./docker-compose.yml -f ./docker-compose.prod.yml logs -f
}