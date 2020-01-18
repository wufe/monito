[CmdletBinding()]
param (
	[Parameter()]
	[string]
	$Branch
)

if ($Branch -eq "") {
	$Branch = "master"
}


$ErrorActionPreference = "Stop";

Write-Host "Deploying..";

#region Check

	$mainEnvPath = "../.env.production";
	$presentationEnvPath = "../Presentation/Monito.Web/appsettings.Production.json"
	$workerEnvPath = "../Presentation/Monito.Worker/.env.production"

	$requiredFiles = $mainEnvPath,$presentationEnvPath,$workerEnvPath

	foreach ($requiredFile in $requiredFiles) {
		$fullPath = Join-Path $PSScriptRoot $requiredFile
		if (!(Test-Path $fullPath)) {
			Write-Host "Could not locate $fullPath. Make sure to create it before deploying.";
			Exit 1
		}
	}

#endregion

#region Load env

	$mainEnvPath = Join-Path $PSScriptRoot $mainEnvPath
	$mainEnvPath = Resolve-Path $mainEnvPath
	
	$env = Get-Content $mainEnvPath

	$lines = $env.Split([Environment]::NewLine)

	$environmentVariables = ${}
	
	foreach ($line in $lines) {
		if ($line.Trim() -ne "") {
			$key, $value = $line.Split("=")
			$environmentVariables.Add($key, $value)
		}
	}

#endregion

#region Stop

	docker-compose -f ./docker-compose.production.yml down

#endregion

#region Latest version

	Write-Host "Upgrading to latest version.."

	git stash
	git fetch origin $Branch
	git checkout $Branch
	git pull --rebase origin $Branch

#endregion

#region Monito.Web

	Write-Host "Building presentation project.."

	Set-Location ./Presentation/Monito.Web

	if (Test-Path ./release) {
		Remove-Item -Recurse -Force ./release
	}

	New-Item -ItemType directory ./release

	yarn
	$env:GA_UA=$environmentVariables.GA_UA; yarn prod

	Set-Location ../../

#endregion

#region Monito.Worker

	Write-Host "Building worker project.."

	Set-Location ./Presentation/Monito.Worker

	if (Test-Path ./release) {
		Remove-Item -Recurse -Force ./release
	}

	New-Item -ItemType directory ./release

	yarn
	yarn prod

	Set-Location ../../

#endregion

#region Start

	Write-Host "Building images and starting.."

	docker-compose -f ./docker-compose.production.yml up -d --build

#endregion

#region Clean

	Write-Host "Cleaning up.."

	docker image prune --force

#endregion

#region Logs

	Set-Location ./Presentation/Monito.Web

	Remove-Item -Recurse -Force release

	Set-Location ../../

	Set-Location ./Presentation/Monito.Worker

	Remove-Item -Recurse -Force release

	Set-Location ../../

	git stash

	docker-compose -f ./docker-compose.production.yml logs -f

#endregion