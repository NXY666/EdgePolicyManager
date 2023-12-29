<p align="center">
  <img src="https://s11.ax1x.com/2023/12/29/piLDqKO.png" alt="Edge Policy Manager" width="60px"/>
</p>
<h1 align="center">Edge Policy Manager</h1>
<p align="center">
    <a href="README.md">简体中文</a> | <a href="README.zh-TW.md">繁體中文</a> | <b>English</b>
</p>
<p align="center">
    <img alt="image" src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/63bb6b03-8188-4c49-8c9e-11c8e9324eb4"/>
</p>

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

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/b6d4edab-2a03-4648-8177-32160d92099c)

### Keyword Search

> Supports fuzzy search of policies by keywords and sorts them by relevance.

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/f7411764-1548-475a-b440-a40beb4025f3)

### Import and Export

> Supports importing and exporting policy configuration files for easy backup and sharing.

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/a3956bef-5071-4c91-8a3e-85bcd36ec521)

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
