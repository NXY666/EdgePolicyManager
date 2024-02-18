<p align="center">
  <img src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/6d2a0952-2101-4906-b82d-58168b4b5f8c" alt="Edge原則管理器" width="60px"/>
</p>
<h1 align="center">Edge 原則管理器</h1>
<p align="center">
    <a href="README.md">简体中文</a> | <b>繁體中文</b> | <a href="README.en-US.md">English</a>
</p>
<p align="center">
    <img alt="Edge原則管理器截圖" src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/230d697f-8a2a-4cdf-a88d-1c2f04a14592"/>
</p>

## 「原則」有什麼用途？

Edge 作為系統內建的瀏覽器，功能繁雜且冗餘。不僅如此，微軟還經常透過彈窗和提示「推薦」那些我們不需要的功能。

透過配置原則，我們可以對 Edge 的功能進行定制，使其更符合我們的使用習慣。

例如：

* 禁用 `EdgeCollectionsEnabled` 原則可關閉 `集錦` 功能。
* 禁用 `ShowPDFDefaultRecommendationsEnabled` 原則可阻止彈出 `將 Microsoft Edge 設為預設 PDF 閱讀程式` 的推薦提示。
* 禁用 `AllowSurfGame` 原則可阻止進入 `衝浪遊戲` 。
* 啟用 `DoubleClickCloseTabEnabled` 原則可啟用雙擊關閉標籤頁功能（僅在中國可用，至少文件是這樣寫的）。

## 特性

### 原則瀏覽與配置

> 支援查看和配置原則，並在未配置的情況下展示通過文件推斷的預設值。

![原則瀏覽與配置截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/63720df0-35d2-4db3-bc2e-e1789fdca361)

### 詳細文件查詢

> 支援查看原則的詳細文件，內容及其翻譯由微軟審核並[提供](https://www.microsoft.com/edge/business/download)。

![詳細文件查詢截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/97e5aaf9-a4a2-4db7-8c1d-4b30ad3e8004)

### 關鍵字模糊檢索

> 支援根據關鍵字模糊搜索原則，並根據相關度排序。

![關鍵字模糊檢索截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9bd48073-2259-4676-9b9d-3800fbe204fb)

### 導入與導出設定檔

> 支援導入和導出原則設定檔，方便備份和共享。

![導入與導出設定檔截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/7fc6e305-334c-4bf4-b185-bda08163638f)

### 多語言支援

> 可用的顯示語言有簡體中文（中國大陸）、繁體中文（中國台灣）、英語（美國）、英語（英國）。

### 登錄檔安全鎖

> 遇到意外的登錄檔路徑時，將阻止對登錄檔的寫入、刪除操作。

## 構建

> 如有需要可以取得儲存庫後自行構建發行版，與 Release 中的發行版完全相同。

```bash
# 取得儲存庫
git clone https://github.com/NXY666/EdgePolicyManager.git

# 進入儲存庫目錄
cd EdgePolicyManager

# 構建發行版（x64）
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# 構建發行版（x86）
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# 構建發行版（ARM64）
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## 說明

* 本工具的程式碼 100% 開源，請遵守開源協議。
* 由於需要編輯登錄檔，工具必須以管理員權限運行，Windows 可能會因此彈出安全警告提示，忽略即可。
* 預設值根據文件內容推斷，僅供參考。如有錯誤可提交 Issue ，將儘快修正。
