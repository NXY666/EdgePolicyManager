import fs from 'fs';
import {XMLParser, XMLBuilder} from 'fast-xml-parser';

(async () => {
	const {PUBLISH_VERSION, CONFIG} = process.env;

	const csprojPath = './PolicyManager.csproj';
	const csprojData = fs.readFileSync(csprojPath);

	// 解析XML
	const options = {
		ignoreAttributes: false,
		attributeNamePrefix: '@',
		suppressEmptyNode: true,
		format: CONFIG === 'Debug'
	};

	const parser = new XMLParser(options);
	const csprojObj = parser.parse(csprojData);

	// 修改版本号
	if (csprojObj.Project?.PropertyGroup) {
		const config = csprojObj.Project.PropertyGroup.find((item) => item["Version"]);
		if (!config) {
			console.log(JSON.stringify(csprojObj.Project.PropertyGroup));
			throw new Error('Cannot find version property.');
		}
		config["Version"] = PUBLISH_VERSION;
	} else {
		console.log(JSON.stringify(csprojObj));
		throw new Error('Cannot find PropertyGroup.');
	}

	// 转换为XML字符串
	const builder = new XMLBuilder(options);
	const xml = builder.build(csprojObj);

	// 写回.csproj文件
	fs.writeFileSync(csprojPath, xml);
})();