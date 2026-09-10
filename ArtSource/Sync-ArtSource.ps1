$RepoRoot = Split-Path $PSScriptRoot -Parent

$RuntimeImages = Join-Path $RepoRoot "Brad8381PlagueBringer\images"
$ArtSource     = $PSScriptRoot
$MappingFile  = Join-Path $ArtSource "ART_MAPPING.txt"

Write-Host "Syncing runtime art into ArtSource..."

# Copy every runtime image directory into ArtSource, preserving structure
Get-ChildItem $RuntimeImages -Recurse -File |
    Where-Object {
        $_.Extension -in ".png", ".jpg", ".jpeg", ".webp", ".svg"
    } |
    ForEach-Object {

        $relative = $_.FullName.Substring($RuntimeImages.Length).TrimStart("\")
        $dest = Join-Path $ArtSource $relative
        $destDir = Split-Path $dest -Parent

        New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        Copy-Item $_.FullName $dest -Force
    }

# Copy mod thumbnail too
$ModImage = Join-Path $RepoRoot "Brad8381PlagueBringer\mod_image.png"

if (Test-Path $ModImage) {
    Copy-Item $ModImage (Join-Path $ArtSource "mod_image.png") -Force
}

# Generate complete mapping/index
$lines = @()

$lines += "PLAGUEBRINGER ART SOURCE INDEX"
$lines += "=============================="
$lines += ""
$lines += "ArtSource is the canonical central location for all artwork."
$lines += ""
$lines += "Runtime assets:"
$lines += "Brad8381PlagueBringer/images/"
$lines += ""
$lines += "------------------------------------------------------------"
$lines += ""

Get-ChildItem $ArtSource -Recurse -File |
    Where-Object {
        $_.Extension -in ".png", ".jpg", ".jpeg", ".webp", ".svg"
    } |
    Sort-Object FullName |
    ForEach-Object {

        $relative = $_.FullName.Substring($ArtSource.Length).TrimStart("\")

        if ($relative -eq "mod_image.png") {
            $runtime = "Brad8381PlagueBringer/mod_image.png"
        }
        else {
            $runtime = "Brad8381PlagueBringer/images/$($relative.Replace('\','/'))"
        }

        $lines += "SOURCE : ArtSource/$($relative.Replace('\','/'))"
        $lines += "RUNTIME: $runtime"
        $lines += ""
    }

$lines += "------------------------------------------------------------"
$lines += ""
$lines += "DEPLOY"
$lines += "------"
$lines += "Run:"
$lines += ".\ArtSource\Deploy-Art.ps1"
$lines += ""
$lines += "SYNC BACK FROM CURRENT MOD"
$lines += "--------------------------"
$lines += "Run:"
$lines += ".\ArtSource\Sync-ArtSource.ps1"

Set-Content $MappingFile $lines -Encoding UTF8

Write-Host ""
Write-Host "ArtSource synchronized."
Write-Host "Mapping generated:"
Write-Host $MappingFile