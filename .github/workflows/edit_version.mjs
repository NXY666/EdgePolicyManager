import fs from 'fs';

(async () => {
	const {PUBLISH_VERSION, CONFIG} = process.env;

	const csprojPath = './PolicyManager.csproj';
	const csprojData = fs.readFileSync(csprojPath).toString();

	const xml = csprojData.replace("0.0.0.0", PUBLISH_VERSION);

	// 写回.csproj文件
	fs.writeFileSync(csprojPath, xml);
})();
