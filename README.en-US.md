# Edge Policy Manager

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/fa759fdd-9882-4cf4-93df-d4d429a3c0cb)

## What is the Use of "Policies"?

As a system's built-in browser, Edge is complex and often redundant. Moreover, Microsoft frequently "recommends" unwanted features through pop-ups and notifications.

By configuring policies, we can customize Edge's functionality to better suit our usage habits.

For example:

* Disabling the `EdgeCollectionsEnabled` policy turns off the `Collections` feature.
* Disabling the `ShowPDFDefaultRecommendationsEnabled` policy stops the "Set Microsoft Edge as your default PDF reader" recommendation pop-ups.
* Disabling the `AllowSurfGame` policy prevents access to the `Surf Game`.
* Enabling the `DoubleClickCloseTabEnabled` policy allows closing tabs with a double-click (available only in China, at least according to the documentation).

## Features

### Policy Browsing and Configuration

> Supports viewing and editing policy configurations, displaying inferred default values in unconfigured cases.

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9a1ef6ea-2020-4d63-8072-e6ef350b2e23)

### Keyword Search

> Supports fuzzy search of policies by keywords and sorts them by relevance.

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/832dfe0c-26fb-480d-8a1a-72d95083211e)

### Import and Export

> Supports importing and exporting policy configuration files for easy backup and sharing.

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/d3663eda-80cd-418d-b76a-1bb63386dbf1)

### Multilingual Support

> Available display languages include `Simplified Chinese (Mainland China)`, `Traditional Chinese (Taiwan)`, `English (United States)`, and `English (United Kingdom)`.

### Security Lock

> Prevents writing to or deleting from the registry when using unexpected registry paths.

## Compilation & Release

> If needed, you can clone the repository and compile it yourself, with functionality identical to the release version.

```bash
# Clone the repository
git clone https://github.com/NXY666/EdgePolicyManager.git

# Enter the repository directory
cd EdgePolicyManager

# Create release version (x64)
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# Create release version (x86)
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# Create release version (ARM64)
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## Notes

* This tool is 100% open source and automatically published through GitHub Action, ensuring no backdoors or virus code. If security issues are found, please submit an Issue.
* Modifying the registry may trigger a security warning from Windows, which can be ignored.
* Default values are inferred from documentation and are for reference only. If errors are found, please submit an Issue for prompt correction.
* The display language always follows the system settings and does not currently support manual switching.
