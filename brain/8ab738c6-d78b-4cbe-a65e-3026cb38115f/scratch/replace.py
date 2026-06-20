import os

file_path = r"C:\Users\hnsil\Documents\GFC\GFC-Studio V2\apps\GFC-Mobile-Standalone\GFC.Mobile\Components\Pages\Liquor\LiquorHubMobile.razor"

with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

# Let's locate the alphabetical view block for Bottles
target = """                                                            <div class="d-flex align-items-center justify-content-center w-100 gap-2 px-3">
                                                                <span class="extra-small fw-bold text-uppercase text-muted" style="font-size: 0.6rem; min-width: 45px; text-align: right;">Bottles</span>
                                                                <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, -1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-dash"></i></button>
                                                                <div class="fw-black px-1 text-center" style="min-width: 30px; font-size: 0.95rem;">@(GetCartQuantity(item.Id) % item.PackSize)</div>
                                                                <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, 1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-plus"></i></button>
                                                            </div>"""

# Normalize target and content line endings to check match
normalized_target = target.replace("\r\n", "\n").strip()
normalized_content = content.replace("\r\n", "\n")

if normalized_target in normalized_content:
    replacement = """                                                            @if (item.CurrentPrice > 0)
                                                            {
                                                                <div class="d-flex align-items-center justify-content-center w-100 gap-2 px-3">
                                                                    <span class="extra-small fw-bold text-uppercase text-muted" style="font-size: 0.6rem; min-width: 45px; text-align: right;">Bottles</span>
                                                                    <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, -1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-dash"></i></button>
                                                                    <div class="fw-black px-1 text-center" style="min-width: 30px; font-size: 0.95rem;">@(GetCartQuantity(item.Id) % item.PackSize)</div>
                                                                    <button class="btn-audit" style="width: 32px; height: 32px; font-size: 0.8rem;" @onclick='() => AdjustCartBottles(item.Id, 1, item.PackSize)' @onclick:stopPropagation="true"><i class="bi bi-plus"></i></button>
                                                                </div>
                                                            }"""
    # Replace and write back matching native newlines
    new_content = normalized_content.replace(normalized_target, replacement.replace("\r\n", "\n"))
    # Match original file line ending style
    if "\r\n" in content:
        new_content = new_content.replace("\n", "\r\n")
    with open(file_path, "w", encoding="utf-8") as f:
        f.write(new_content)
    print("Replacement successful!")
else:
    print("Target not found. Let's inspect the target again.")
