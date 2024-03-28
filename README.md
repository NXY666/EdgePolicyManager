<p align="center">
  <img src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/6d2a0952-2101-4906-b82d-58168b4b5f8c" alt="Edge Policy Manager" width="60px"/>
</p>
<h1 align="center">Edge 策略管理器</h1>
<p align="center">
    <b>简体中文</b> | <a href="README.zh-TW.md">繁體中文</a> | <a href="README.en-US.md">English</a>
</p>
<p align="center">
    <img alt="Edge策略管理器截图" src="https://github.com/NXY666/EdgePolicyManager/assets/62371554/230d697f-8a2a-4cdf-a88d-1c2f04a14592"/>
</p>

## “策略”有什么用？

Edge 作为系统自带的浏览器，功能复杂且冗余。不仅如此，微软还经常通过弹窗和提示“推荐”那些我们不需要的功能。

通过配置策略，我们可以对 Edge 的功能进行定制，使其更符合我们的使用习惯。

例如：

* 禁用 `EdgeCollectionsEnabled` 策略可关闭 `集锦` 功能。
* 禁用 `ShowPDFDefaultRecommendationsEnabled` 策略可阻止弹出 `将 Microsoft Edge 设置为默认 PDF 阅读器` 的推荐提示。
* 禁用 `AllowSurfGame` 策略可阻止进入 `冲浪游戏` 。
* 启用 `DoubleClickCloseTabEnabled` 策略可启用双击关闭标签页功能（仅在中国可用，至少文档是这样写的）。

更多策略推荐详见[博客](https://blog.csdn.net/NXY666/article/details/135984889)。

## 特性

### 策略浏览与配置

> 支持在未配置的情况下展示通过文档推断的默认值。

![策略浏览与配置截图](https://github.com/NXY666/EdgePolicyManager/assets/62371554/63720df0-35d2-4db3-bc2e-e1789fdca361)

### 详细文档查询

> 内容及其翻译由微软审核并[提供](https://www.microsoft.com/edge/business/download)。

![详细文档查询截图](https://github.com/NXY666/EdgePolicyManager/assets/62371554/97e5aaf9-a4a2-4db7-8c1d-4b30ad3e8004)

### 关键字模糊检索

> 搜索结果根据相关度排序。

![关键字模糊检索截图](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9bd48073-2259-4676-9b9d-3800fbe204fb)

### 导入与导出配置文件

> 方便备份和共享。

![导入与导出配置文件截图](https://github.com/NXY666/EdgePolicyManager/assets/62371554/7fc6e305-334c-4bf4-b185-bda08163638f)

### 注册表安全锁

> 遇到意外的注册表路径时，将阻止对注册表的写入、删除操作。

### 多语言

> 可用的显示语言有简体中文（中国大陆）、繁体中文（中国台湾）、英语（美国）、英语（英国）。

### 深色模式

> 主题颜色随系统色彩模式自动切换。

## 构建

> 如有需要可以克隆仓库后自行构建发行版，与 Release 中的发行版完全相同。

```bash
# 克隆仓库
git clone https://github.com/NXY666/EdgePolicyManager.git

# 进入仓库目录
cd EdgePolicyManager

# 构建发行版（x64）
dotnet publish -p:Platform=x64 -p:PublishProfile=Properties/PublishProfiles/win-x64.pubxml

# 构建发行版（x86）
dotnet publish -p:Platform=x86 -p:PublishProfile=Properties/PublishProfiles/win-x86.pubxml

# 构建发行版（ARM64）
dotnet publish -p:Platform=ARM64 -p:PublishProfile=Properties/PublishProfiles/win-arm64.pubxml
```

## 说明

* 本工具代码 100% 开源，请遵守开源协议。
* 由于需要编辑注册表，工具必须以管理员权限运行，Windows 可能会因此弹出安全警告提示，忽略即可。
* 默认值根据文档内容推断，仅供参考。如有错误可提交 Issue ，将尽快修正。
