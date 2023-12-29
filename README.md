<p align="center">
  <img src="https://s11.ax1x.com/2023/12/29/piLDqKO.png" alt="Edge Policy Manager" width="60px"/>
</p>
<h1 align="center">Edge 策略管理器</h1>
<p align="center">
    <b>简体中文</b> | <a href="README.zh-TW.md">繁體中文</a> | <a href="README.en-US.md">English</a>
</p>
<p align="center">
    <img alt="image" src="https://s11.ax1x.com/2023/12/29/piL6mid.png"/>
</p>

## “策略”有什么用？

Edge 作为系统自带的浏览器，功能复杂且冗余。不仅如此，微软还经常通过弹窗和提示“推荐”那些我们不需要的功能。

通过配置策略，我们可以对 Edge 的功能进行定制，使其更符合我们的使用习惯。

例如：

* 禁用 `EdgeCollectionsEnabled` 策略可关闭 `集锦` 功能。
* 禁用 `ShowPDFDefaultRecommendationsEnabled` 策略可阻止弹出“将 Microsoft Edge 设置为默认 PDF 阅读器”的推荐提示。
* 禁用 `AllowSurfGame` 策略可阻止进入 `冲浪游戏` 。
* 启用 `DoubleClickCloseTabEnabled` 策略可启用双击关闭标签页功能（仅在中国可用，至少文档是这样写的）。

## 特性

### 策略浏览与配置

> 支持查看和编辑策略配置，在未配置的情况下展示通过文档推断的默认值。

![image](https://s11.ax1x.com/2023/12/29/piLcpTg.png)

### 关键字检索

> 支持根据关键字模糊搜索策略，并根据相关度排序。

![image](https://s11.ax1x.com/2023/12/29/piLcifs.png)

### 导入与导出

> 支持导入和导出策略配置文件，方便备份和共享。

![image](https://s11.ax1x.com/2023/12/29/piLcKk4.png)

### 多语言支持

> 可用的显示语言有 `简体中文（中国大陆）` 、 `繁体中文（中国台湾）` 、 `英语（美国）` 、 `英语（英国）`。

### 安全锁

> 使用意外的注册表路径时，将阻止对注册表的写入、删除操作。

## 编译 & 发布

> 如有需要可以克隆仓库后自行编译使用，与发行版功能完全相同。

```bash
# 克隆仓库
git clone https://github.com/NXY666/EdgePolicyManager.git

# 进入仓库目录
cd EdgePolicyManager

# 创建发布版（x64）
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# 创建发布版（x86）
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# 创建发布版（ARM64）
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## 说明

* 本工具 100% 开源，并通过 GitHub Action 自动发布，不可能存在后门或病毒代码，如发现安全问题请提交 Issue 。
* 由于需要修改注册表，Windows 可能会弹出安全警告提示，忽略即可。
* 默认值根据文档内容推断，仅供参考。如有错误可提交 Issue ，将尽快修正。
* 显示语言始终跟随系统，暂不支持主动切换。
