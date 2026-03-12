import sys

with open(r"c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\Components\Pages\Dashboard.razor", "r", encoding="utf-8") as f:
    text = f.read()

# Only count until @code
markup = text.split("@code")[0]
opens = markup.count("<div")
closes = markup.count("</div")
with open("div_count.txt", "w") as f2:
    f2.write(f"Opens: {opens}, Closes: {closes}")
print(f"Opens: {opens}, Closes: {closes}")
