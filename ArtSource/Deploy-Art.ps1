$RepoRoot = Split-Path $PSScriptRoot -Parent

$RuntimeImages = Join-Path $RepoRoot "Brad8381PlagueBringer\images"

$Folders = @(
    "card_portraits",
    "character",
    "charui",
    "potions",
    "powers",
    "relics"
)

Write-Host "Deploying ArtSource -> runtime..."

foreach ($folder in $Folders) {

    $src = Join-Path $PSScriptRoot $folder
    $dst = Join-Path $RuntimeImages $folder

    if (-not (Test-Path $src)) {
        continue
    }

    Get-ChildItem $src -Recurse -File |
        Where-Object {
            $_.Extension -in ".png", ".jpg", ".jpeg", ".webp", ".svg"
        } |
        ForEach-Object {

            $relative = $_.FullName.Substring($src.Length).TrimStart("\")
            $dest = Join-Path $dst $relative
            $destDir = Split-Path $dest -Parent

            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
            Copy-Item $_.FullName $dest -Force
        }
}

$ModImage = Join-Path $PSScriptRoot "mod_image.png"

if (Test-Path $ModImage) {
    Copy-Item $ModImage `
        (Join-Path $RepoRoot "Brad8381PlagueBringer\mod_image.png") `
        -Force
}

Write-Host "Art deployed."