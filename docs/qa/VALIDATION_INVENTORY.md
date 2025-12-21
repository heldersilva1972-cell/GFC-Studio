# Validation Inventory

This document serves as the "Approved Source of Truth" for all business rules, logic constraints, and data validation within the GFC-Studio application.

| File/Component | Rule Name | Expected Behavior |
|---|---|---|
| **GFC.Core/BusinessRules/MemberStatusHelper.cs** | `GetAllowedStatusesForMember` | Determines the valid next statuses for a member based on their current status (e.g., a `GUEST` can become a `REGULAR`, `INACTIVE`, or `REJECTED`). |
| | `IsLifeEligible` | A member is eligible for `LIFE` status if they are at least 65 years old and have been a `REGULAR` member for at least 15 years. |
| | `IsLifeStatus` | A member's status is considered a `LIFE` status if it is "LIFE" or "LIFE MEMBER". |
| | `NormalizeStatus` | Normalizes member status strings to ensure consistency (e.g., "LIFE MEMBER" becomes "LIFE", "REGULARNP" becomes "REGULAR-NP"). |
| **GFC.Core/BusinessRules/DuesWaiverHelper.cs** | `IsLifeMemberForYear` | A member is considered a life member for a given year if their `LifeEligibleDate` or `StatusChangeDate` is on or before that year. |
| | `RevertServiceWaiverForYear` | A service waiver can only be reverted if the member is not a life or board member for the given year, and the payment record is a service waiver. |
| **GFC.Core/BusinessRules/MemberFilters.cs** | `IsActiveForOperationalViews` | A member is considered "active" for operational views if their status is `GUEST`, `REGULAR`, `REGULAR-NP`, or `LIFE`. |
| | `IsActiveForDuesYear` | A member is active for a given dues year if they are active for operational views and do not have an inactive or death date in a prior year. |
| **GFC.Core/Services/PasswordPolicy.cs** | `Validate` | Passwords must be at least 10 characters long, contain a mix of uppercase, lowercase, and numbers or symbols, not contain the username, and not be a common password. |
| **GFC.BlazorServer/Auth/AppAuthorization.cs** | `AppPolicies.RequireAdmin` | The `RequireAdmin` policy requires that the user has the `Admin` role. |
| **GFC.BlazorServer/Components/Pages/AddMember.razor** | `MemberFormModel` | `FirstName`, `LastName`, `Address1`, `City`, `State`, `PostalCode`, `Status`, `ApplicationDate`, and `DateOfBirth` are all required fields. |
| | `MemberFormModel` | `FirstName` and `LastName` cannot exceed 50 characters. |
| | `MemberFormModel` | `State` must be exactly 2 characters. |
| | `MemberFormModel` | `Email` must be a valid email address format. |
| | `SaveMember` | At least one phone number (Home or Cell) is required. |
| | `SaveMember` | A new member with the `GUEST` status is automatically marked as non-Portuguese. |
| **GFC.BlazorServer/Components/Pages/EditMember.razor** | `MemberFormModel` | `FirstName`, `LastName`, `Address1`, `City`, `State`, `PostalCode`, `Status`, `ApplicationDate`, and `DateOfBirth` are all required fields. |
| | `MemberFormModel` | `FirstName` and `LastName` cannot exceed 50 characters. |
| | `MemberFormModel` | `State` must be exactly 2 characters. |
| | `MemberFormModel` | `Email` must be a valid email address format. |
| | `SaveMember` | At least one phone number (Home or Cell) is required. |
| **GFC.BlazorServer/Components/Pages/ChangePassword.razor** | `HandleChangePassword` | A new password is required. |
| | `HandleChangePassword` | The new password and confirmation password must match. |
| | `HandleChangePassword` | The new password must adhere to the password policy. |
| **GFC.BlazorServer/Components/Pages/UserManagement.razor** | `UserFormModel` | `Username` is a required field. |
| | `SaveUser` | The admin username cannot be changed. |
| | `SaveUser` | The admin user must remain an administrator. |
| | `SaveUser` | The admin user must remain active. |
| | `SaveUser` | Usernames must be unique. |
| | `SaveUser` | A member can only be associated with one user account. |
| | `SaveUser` | A password is required for new users. |
| | `SaveUser` | Passwords must adhere to the password policy. |
| | `DeleteUserConfirmed` | The admin user cannot be deactivated. |
