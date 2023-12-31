<p align="center">
  <img src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/6d2a0952-2101-4906-b82d-58168b4b5f8c" alt="Edge策略管理器" width="60px"/>
</p>
<h1 align="center">Edge 策略管理器</h1>
<p align="center">
    <a href="README.md">简体中文</a> | <b>繁體中文</b> | <a href="README.en-US.md">English</a>
</p>
<p align="center">
    <img alt="Edge策略管理器截圖" src="https://s11.ax1x.com/2023/12/29/piL6mid.png"/>
</p>

## 「策略」有什麼用途？

Edge 作為系統內建的瀏覽器，功能繁雜且冗餘。不僅如此，微軟還經常透過彈窗和提示「推薦」那些我們不需要的功能。

透過配置策略，我們可以對 Edge 的功能進行定制，使其更符合我們的使用習慣。

例如：

* 禁用 `EdgeCollectionsEnabled` 策略可關閉 `集錦` 功能。
* 禁用 `ShowPDFDefaultRecommendationsEnabled` 策略可阻止彈出 `將 Microsoft Edge 設為預設 PDF 閱讀程式` 的推薦提示。
* 禁用 `AllowSurfGame` 策略可阻止進入 `衝浪遊戲` 。
* 啟用 `DoubleClickCloseTabEnabled` 策略可啟用雙擊關閉標籤頁功能（僅在中國可用，至少文檔是這樣寫的）。

## 特性

### 策略瀏覽與配置

> 支援查看和配置策略，並在未配置的情況下展示通過文檔推斷的預設值。

![策略瀏覽與配置截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/ccaf628d-1ee4-42f4-9e58-8fe47b4a80fa)

### 詳細文檔查詢

> 支援查看策略的詳細文檔，內容及其翻譯由微軟審核並[提供](https://www.microsoft.com/edge/business/download)。

![詳細文檔查詢截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/8094142f-8e81-4b66-8803-77142dde5aee)

### 關鍵字模糊檢索

> 支援根據關鍵字模糊搜索策略，並根據相關度排序。

![關鍵字模糊檢索截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/f7411764-1548-475a-b440-a40beb4025f3)

### 匯入與匯出配置文件

> 支援匯入和匯出策略配置文件，方便備份和共享。

![匯入與匯出配置文件截圖](https://github.com/NXY666/EdgePolicyManager/assets/62371554/2fd2a50c-055d-4900-81cf-fce2b5f5fc23)

### 多語言支援

> 可用的顯示語言有簡體中文（中國大陸）、繁體中文（中國台灣）、英語（美國）、英語（英國）。

### 註冊表安全鎖

> 遇到意外的註冊表路徑時，將阻止對註冊表的寫入、刪除操作。

## 構建

> 如有需要可以克隆倉庫後自行構建發行版，與 Release 中的發行版完全相同。

```bash
# 克隆倉庫
git clone https://github.com/NXY666/EdgePolicyManager.git

# 進入倉庫目錄
cd EdgePolicyManager

# 構建發行版（x64）
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# 構建發行版（x86）
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# 構建發行版（ARM64）
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## 說明

* 本工具 100% 開源，並通過 GitHub Action 自動構建並發布，不會也不可能存在後門或病毒代碼。
* 由於需要修改註冊表，工具必須以管理員權限運行，Windows 可能會因此彈出安全警告提示，忽略即可。
* 預設值根據文檔內容推斷，僅供參考。如有錯誤可提交 Issue ，將儘快修正。
* 顯示語言始終跟隨系統，暫不支援主動切換。
