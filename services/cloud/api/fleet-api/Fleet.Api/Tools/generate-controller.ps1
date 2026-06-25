# generate-controller.ps1
#
# Purpose:
#   Generate ASP.NET Core API controllers with parameterized inputs.
#
# Usage:
#   .\generate-controller.ps1 `
#       -ControllerName SitesController `
#       -ModelFqn Fleet.Domain.Entities.Metadata.Site `
#       -DbContextFqn Fleet.Infrastructure.Persistence.FleetDbContext

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ControllerName,

    [Parameter(Mandatory = $true)]
    [string]$ModelFqn,

    [Parameter(Mandatory = $true)]
    [string]$DbContextFqn
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Validate controller naming convention
if ($ControllerName -notmatch 'Controller$')
{
    Write-Warning "Controller name should end with 'Controller'"
}

Write-Host "Generating controller..." -ForegroundColor Green
Write-Host ""
Write-Host "Controller: $ControllerName"
Write-Host "Model:      $ModelFqn"
Write-Host "DbContext:  $DbContextFqn"
Write-Host ""

& dotnet aspnet-codegenerator controller `
    -name $ControllerName `
    -async `
    -api `
    -m $ModelFqn `
    -dc $DbContextFqn `
    -dbProvider postgres `
    -outDir Controllers

Write-Host ""
Write-Host "✓ Controller generation complete. Review the output above for details!" -ForegroundColor Green
