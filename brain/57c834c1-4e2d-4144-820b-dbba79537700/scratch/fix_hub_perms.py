import re

file_path = r"c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\GFC-Mobile-Standalone\GFC.Mobile\Components\Pages\MobileHub.razor"

with open(file_path, "r", encoding="utf-8") as f:
    content = f.read()

# Target the specific block in OnInitializedAsync
pattern = r'_userPermissions = UserService\.GetUserPagePermissions\(currentAppUser\.UserId\);\s+UserRoutes = _userPermissions\.Where\(p => p\.CanAccess\)\.Select\(p => \(p\.PageRoute \?\? ""\)\.Trim\(\'/\'\)\.ToLowerInvariant\(\)\)\.ToHashSet\(\);\s+UserPageNames = _userPermissions\.Where\(p => p\.CanAccess\)\.Select\(p => \(p\.PageName \?\? ""\)\.Trim\(\)\.ToLowerInvariant\(\)\)\.ToHashSet\(\);'

replacement = """_userPermissions = UserService.GetUserPagePermissions(currentAppUser.UserId);
                
                // [FORCE UPDATE] Strictly filter dashboard tiles to MOBILE HUB category
                UserPageNames = _userPermissions
                    .Where(p => p.CanAccess && (p.Category?.ToUpper() == "MOBILE HUB" || (p.PageName != null && p.PageName.Contains("(Mobile)"))))
                    .Select(p => (p.PageName ?? "").Trim().ToLowerInvariant())
                    .ToHashSet();

                UserRoutes = _userPermissions
                    .Where(p => p.CanAccess)
                    .Select(p => (p.PageRoute ?? "").Trim('/').ToLowerInvariant())
                    .ToHashSet();"""

new_content = re.sub(pattern, replacement, content)

with open(file_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print("Replacement complete.")
