$RepoRoot = Split-Path $PSScriptRoot -Parent

$RuntimeImages = Join-Path $RepoRoot "PlagueBringer\images"
$ArtSource     = $PSScriptRoot
$MappingFile  = Join-Path $ArtSource "ART_MAPPING.txt"

$Folders = @(
    "card_portraits",
    "character",
    "charui",
    "potions",
    "powers",
    "relics"
)

Write-Host "Syncing runtime art into ArtSource..."

foreach ($folder in $Folders) {

    $src = Join-Path $RuntimeImages $folder
    $dst = Join-Path $ArtSource $folder

    if (-not (Test-Path $src)) {
        continue
    }

    New-Item -ItemType Directory -Path $dst -Force | Out-Null

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

$ModImage = Join-Path $RepoRoot "PlagueBringer\mod_image.png"

if (Test-Path $ModImage) {
    Copy-Item $ModImage (Join-Path $ArtSource "mod_image.png") -Force
}

$lines = @(
    "PLAGUEBRINGER ART SOURCE INDEX",
    "==============================",
    "",
    "ArtSource is the canonical central location for all artwork.",
    "",
    "Runtime assets:",
    "PlagueBringer/images/",
    ""
)

foreach ($folder in $Folders) {

    $sourceFolder = Join-Path $ArtSource $folder

    if (-not (Test-Path $sourceFolder)) {
        continue
    }

    Get-ChildItem $sourceFolder -Recurse -File |
        Where-Object {
            $_.Extension -in ".png", ".jpg", ".jpeg", ".webp", ".svg"
        } |
        Sort-Object FullName |
        ForEach-Object {

            $relative = $_.FullName.Substring($ArtSource.Length).TrimStart("\")
            $unix = $relative.Replace("\", "/")

            $lines += "SOURCE : ArtSource/$unix"
            $lines += "RUNTIME: PlagueBringer/images/$unix"
            $lines += ""
        }
}

$lines += "SOURCE : ArtSource/mod_image.png"
$lines += "RUNTIME: PlagueBringer/mod_image.png"
$lines += ""
$lines += "Legacy art is stored in ArtSource/legacy and is NOT deployed."

Set-Content $MappingFile $lines -Encoding UTF8

Write-Host "ArtSource synchronized."