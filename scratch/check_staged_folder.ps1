$stagedDir = "c:\Users\hnsil\Documents\GFC\GFC-Studio V2\apps\webapp\App_Data\StagedReports"
if (Test-Path $stagedDir) {
    $files = Get-ChildItem $stagedDir -Filter "*.csv"
    Write-Host "Total CSV files in StagedReports folder:" $files.Count
    foreach ($f in $files) {
        Write-Host $f.Name
    }
} else {
    Write-Host "StagedReports directory does not exist."
}
