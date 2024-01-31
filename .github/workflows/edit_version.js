const fs = require('fs');
const exec = require("child_process");

(async () => {
	// 安装依赖
	exec.execSync('npm install fast-xml-parser');
	
	const {XMLParser, XMLBuilder} = require('fast-xml-parser');
	
	const {PUBLISH_VERSION} = process.argv;

	const csprojPath = './PolicyManager.csproj';
	const csprojData = fs.readFileSync(csprojPath, 'utf-8');

	// 解析XML
	const options = {
		ignoreAttributes: false,
		attributeNamePrefix: '@',
		suppressEmptyNode: true,
		format: true
	};

	const parser = new XMLParser(options);
	const csprojObj = parser.parse(csprojData);

	// 修改版本号
	if (csprojObj.Project?.PropertyGroup?.[0]) {
		csprojObj.Project.PropertyGroup[0].Version = PUBLISH_VERSION;
	}

	// 转换为XML字符串
	const builder = new XMLBuilder(options);
	const xml = builder.build(csprojObj);

	// 写回.csproj文件
	fs.writeFileSync(csprojPath, xml);
})();