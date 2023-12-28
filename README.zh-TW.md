# Edge 策略管理器

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/fa759fdd-9882-4cf4-93df-d4d429a3c0cb)

## “策略”的用途是什麼？

作為系統自帶的瀏覽器，Edge 功能複雜且冗餘。此外，微軟還經常通過彈窗和提示“推薦”那些我們不需要的功能。

通過配置策略，我們可以對 Edge 的功能進行定制，使其更符合我們的使用習慣。

例如：

* 禁用 `EdgeCollectionsEnabled` 策略可關閉 `集錦` 功能。
* 禁用 `ShowPDFDefaultRecommendationsEnabled` 策略可阻止彈出“將 Microsoft Edge 設置為預設 PDF 閱讀器”的推薦提示。
* 禁用 `AllowSurfGame` 策略可阻止進入 `衝浪遊戲` 。
* 啟用 `DoubleClickCloseTabEnabled` 策略可啟用雙擊關閉標籤頁功能（僅在中國可用，至少文檔是這樣寫的）。

## 特色

### 策略瀏覽與配置

> 支援查看和編輯策略配置，在未配置的情況下展示通過文檔推斷的預設值。

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9a1ef6ea-2020-4d63-8072-e6ef350b2e23)

### 關鍵字檢索

> 支援根據關鍵字模糊搜索策略，並根據相關度排序。

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/832dfe0c-26fb-480d-8a1a-72d95083211e)

### 導入與導出

> 支援導入和導出策略配置文件，方便備份和共享。

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/d3663eda-80cd-418d-b76a-1bb63386dbf1)

### 多語言支援

> 可用的顯示語言有 `簡體中文（中國大陸）` 、 `繁體中文（中國台灣）` 、 `英語（美國）` 、 `英語（英國）`。

### 安全鎖

> 使用意外的註冊表路徑時，將阻止對註冊表的寫入、刪除操作。

## 編譯 & 發布

> 如有需要可以克隆倉庫後自行編譯使用，與發行版功能完全相同。

```bash
# 克隆倉庫
git clone https://github.com/NXY666/EdgePolicyManager.git

# 進入倉庫目錄
cd EdgePolicyManager

# 建立發布版（x64）
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# 建立發布版（x86）
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# 建立發布版（ARM64）
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## 說明

* 本工具 100% 開源，並通過 GitHub Action 自動發布，不可能存在後門

或病毒代碼，如發現安全問題請提交 Issue。
* 由於需要修改註冊表，Windows 可能會彈出安全警告提示，忽略即可。
* 預設值根據文檔內容推斷，僅供參考。如有錯誤可提交 Issue，將儘快修正。
* 顯示語言始終跟隨系統，暫不支援主動切換。
