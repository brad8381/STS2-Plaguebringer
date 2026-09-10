$RepoRoot = Split-Path $PSScriptRoot -Parent

$Source = Join-Path $PSScriptRoot "Cards"
$Destination = Join-Path $RepoRoot "Brad8381PlagueBringer\images\card_portraits"

Write-Host "Deploying card art..."

Copy-Item "$Source\*.png" $Destination -Force

Write-Host "Done."