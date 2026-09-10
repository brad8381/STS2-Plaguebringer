$RepoRoot = Split-Path $PSScriptRoot -Parent

$RuntimeImages = Join-Path $RepoRoot "Brad8381PlagueBringer\images"

Write-Host "Deploying ArtSource -> runtime..."

Get-ChildItem $PSScriptRoot -Recurse -File |
    Where-Object {
        $_.Extension -in ".png", ".jpg", ".jpeg", ".webp", ".svg" -and
        $_.Name -ne "mod_image.png"
    } |
    ForEach-Object {

        $relative = $_.FullName.Substring($PSScriptRoot.Length).TrimStart("\")
        $dest = Join-Path $RuntimeImages $relative
        $destDir = Split-Path $dest -Parent

        New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        Copy-Item $_.FullName $dest -Force
    }

$ModImage = Join-Path $PSScriptRoot "mod_image.png"

if (Test-Path $ModImage) {
    Copy-Item $ModImage `
        (Join-Path $RepoRoot "Brad8381PlagueBringer\mod_image.png") `
        -Force
}

Write-Host "Art deployed."