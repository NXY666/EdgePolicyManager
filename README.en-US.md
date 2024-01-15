<p align="center">
  <img src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/6d2a0952-2101-4906-b82d-58168b4b5f8c" alt="Edge Policy Manager" width="60px"/>
</p>
<h1 align="center">Edge Policy Manager</h1>
<p align="center">
    <a href="README.md">简体中文</a> | <a href="README.zh-TW.md">繁體中文</a> | <b>English</b>
</p>
<p align="center">
    <img alt="Edge Policy Manager Screenshot" src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/230d697f-8a2a-4cdf-a88d-1c2f04a14592"/>
</p>

## What is "Policy" Used For?

As a system's built-in browser, Edge is complex and often redundant. Moreover, Microsoft frequently "recommends" unwanted features through pop-ups and notifications.

By configuring policies, we can customize Edge's functionality to better suit our usage habits.

For example:

* Disabling the `EdgeCollectionsEnabled` policy can turn off the `Collections` feature.
* Disabling the `ShowPDFDefaultRecommendationsEnabled` policy can prevent the pop-up recommendation to `Set Microsoft Edge as default PDF reader` .
* Disabling the `AllowSurfGame` policy can prevent access to the `Surf Game` .
* Enabling the `DoubleClickCloseTabEnabled` policy can enable the double-click tab closing feature (only available in China, at least according to the documentation).

## Features

### Policy Browsing and Configuration

> Supports viewing and configuring policies, and displays default values inferred from the documentation when not configured.

![Policy Browsing and Configuration Screenshot](https://github.com/NXY666/EdgePolicyManager/assets/62371554/63720df0-35d2-4db3-bc2e-e1789fdca361)

### Detailed Documentation Lookup

> Supports viewing detailed documentation for policies, content and translations are reviewed by Microsoft and [provided](https://www.microsoft.com/edge/business/download).

![Detailed Documentation Lookup Screenshot](https://github.com/NXY666/EdgePolicyManager/assets/62371554/97e5aaf9-a4a2-4db7-8c1d-4b30ad3e8004)

### Keyword Fuzzy Search

> Supports searching policies based on keywords and sorts them by relevance.

![Keyword Fuzzy Search Screenshot](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9bd48073-2259-4676-9b9d-3800fbe204fb)

### Import and Export Configuration Files

> Supports importing and exporting policy configuration files for easy backup and sharing.

![Import and Export Configuration Files Screenshot](https://github.com/NXY666/EdgePolicyManager/assets/62371554/7fc6e305-334c-4bf4-b185-bda08163638f)

### Multilingual Support

> Available display languages include Simplified Chinese (Mainland China), Traditional Chinese (Taiwan), English (United States), and English (United Kingdom).

### Registry Security Lock

> When encountering unexpected registry paths, it will prevent write and delete operations on the registry.

## Building

> If needed, you can clone the repository and build the release version yourself, which is identical to the one in the Releases.

```bash
# Clone the repository
git clone https://github.com/NXY666/EdgePolicyManager.git

# Enter the repository directory
cd EdgePolicyManager

# Build the release version (x64)
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# Build the release version (x86)
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# Build the release version (ARM64)
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## Notes

* This tool is 100% open-source and is automatically built and released via GitHub Actions, ensuring that there are no backdoors or virus code.
* As it requires modifying the registry, the tool must run with administrator privileges, and Windows may display security warnings, which can be ignored.
* Default values are inferred from the documentation and are for reference only. If there are errors, please submit an issue, and they will be corrected as soon as possible.
