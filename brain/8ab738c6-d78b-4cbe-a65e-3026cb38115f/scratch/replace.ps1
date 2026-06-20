$p = "C:\Users\hnsil\Documents\GFC\GFC-Studio V2\apps\GFC-Mobile-Standalone\GFC.Mobile\Components\Pages\Liquor\LiquorHubMobile.razor"
$c = [System.IO.File]::ReadAllText($p, [System.Text.Encoding]::UTF8)

# Locate the first block containing AdjustCartBottles but not containing @if (item.CurrentPrice > 0)
# We can find all matches of the Bottles container div in the alphabetical view and replace the first one.

$target = @"
                                                            <div class="d-flex align-items-center justify-content-center w-100 gap-2 px-3">
                                                                <span class="extra-small fw-bold text-uppercase text-muted" style="font-size: 0.6rem; min-width: 45px; text-align: right;">Bottles</span>
                                                                <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, -1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-dash"></i></button>
                                                                <div class="fw-black px-1 text-center" style="min-width: 30px; font-size: 0.95rem;">@(GetCartQuantity(item.Id) % item.PackSize)</div>
                                                                <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, 1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-plus"></i></button>
                                                            </div>
"@

$replacement = @"
                                                            @if (item.CurrentPrice > 0)
                                                            {
                                                                <div class="d-flex align-items-center justify-content-center w-100 gap-2 px-3">
                                                                    <span class="extra-small fw-bold text-uppercase text-muted" style="font-size: 0.6rem; min-width: 45px; text-align: right;">Bottles</span>
                                                                    <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, -1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-dash"></i></button>
                                                                    <div class="fw-black px-1 text-center" style="min-width: 30px; font-size: 0.95rem;">@(GetCartQuantity(item.Id) % item.PackSize)</div>
                                                                    <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, 1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-plus"></i></button>
                                                                </div>
                                                            }
"@

# Normalize newlines
$c_norm = $c -replace "`r`n", "`n"
$target_norm = $target -replace "`r`n", "`n"
$replacement_norm = $replacement -replace "`r`n", "`n"

if ($c_norm.Contains($target_norm)) {
    # Replace only the first occurrence (alphabetical view)
    $index = $c_norm.IndexOf($target_norm)
    $new_content = $c_norm.Substring(0, $index) + $replacement_norm + $c_norm.Substring($index + $target_norm.Length)
    
    # Save back matching original line endings
    if ($c.Contains("`r`n")) {
        $new_content = $new_content -replace "`n", "`r`n"
    }
    
    [System.IO.File]::WriteAllText($p, $new_content, [System.Text.Encoding]::UTF8)
    Write-Output "Successfully updated standalone LiquorHubMobile.razor!"
} else {
    Write-Output "Target Bottles div not found."
}
