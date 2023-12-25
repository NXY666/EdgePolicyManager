# Edge 策略管理器

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/fa759fdd-9882-4cf4-93df-d4d429a3c0cb)

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

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/9a1ef6ea-2020-4d63-8072-e6ef350b2e23)

### 关键字检索

> 支持根据关键字模糊搜索策略，并根据相关度排序。

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/832dfe0c-26fb-480d-8a1a-72d95083211e)

### 导入/导出策略配置

> 支持导入/导出策略配置文件，方便备份和共享。

![image](https://github.com/NXY666/EdgePolicyManager/assets/62371554/d3663eda-80cd-418d-b76a-1bb63386dbf1)

### 多语言支持

> 可用的显示语言有 `简体中文` 、 `繁体中文（台湾）` 、 `英语` 。

### 安全锁

> 使用意外的注册表路径时，将阻止对注册表的写入、删除操作。

## 说明

* 本软件 100% 开源，发布版通过 GitHub Action 自动发布，如不放心可自行编译使用。
* 由于需要修改注册表，Windows 可能会弹出安全警告提示，忽略即可。
* 默认值根据文档内容推断，仅供参考。
* 暂不支持主动切换显示语言。
