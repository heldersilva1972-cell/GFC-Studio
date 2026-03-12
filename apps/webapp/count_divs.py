import sys

with open(r"c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\Components\Pages\Operations\EndOfShiftSales.razor", "r", encoding="utf-8") as f:
    text = f.read()

opens = text.count("<div")
closes = text.count("</div")

print(f"Opens: {opens}")
print(f"Closes: {closes}")
print(f"Difference: {opens - closes}")
