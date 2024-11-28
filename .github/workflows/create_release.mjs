import fs from 'fs';
import {execSync} from 'child_process';
import {promisify} from 'util';
import github from '@actions/github';
import {Octokit} from '@octokit/rest';

function notice(msg) {
	console.log(`* ${msg}`);
}

function retry(fn, retryCount = 0) {
	return fn().catch((e) => {
		if (retryCount >= 3) {
			throw e;
		}
		console.error(e);
		console.warn(`第 ${retryCount + 1} 次执行失败，正在重试……`);
		return retry(fn, retryCount + 1);
	});
}

(async () => {
	const {
		GITHUB_TOKEN,
		GITEE_TOKEN,
		PUBLISH_VERSION,
		EDGE_POLICY_VERSION,
		PUBLISH_DATETIME,
		COMMIT_TITLE,
		COMMIT_BODY,
		CONFIG
	} = process.env;

	const owner = github.context.repo.owner;
	const repo = github.context.repo.repo;
	
	execSync("dotnet restore");
	notice(`还原完成。`);

	await Promise.all(['x64', 'x86', 'arm64'].map(async (arch) => {
		await Promise.all([
			promisify(execSync)(`dotnet publish -p:Platform=${arch} -p:PublishProfile=Properties/PublishProfiles/win-${arch}.pubxml`),
			promisify(execSync)(`dotnet publish -p:Platform=${arch} -p:PublishProfile=Properties/PublishProfiles/win-${arch}-lite.pubxml`)
		]);
		notice(`${arch} 架构软件包生成完成。`);
	}));

	const files = [
		{
			name: 'EdgePolicyManager-v${PUBLISH_VERSION}-x64.exe',
			path: './bin/publish/win-x64/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		},
		{
			name: 'EdgePolicyManager-v${PUBLISH_VERSION}-x86.exe',
			path: './bin/publish/win-x86/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		},
		{
			name: 'EdgePolicyManager-v${PUBLISH_VERSION}-arm64.exe',
			path: './bin/publish/win-arm64/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		},
		{
			name: 'EdgePolicyManager-RuntimeRequired-v${PUBLISH_VERSION}-x64.exe',
			path: './bin/publish/win-x64-lite/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		},
		{
			name: 'EdgePolicyManager-RuntimeRequired-v${PUBLISH_VERSION}-x86.exe',
			path: './bin/publish/win-x86-lite/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		},
		{
			name: 'EdgePolicyManager-RuntimeRequired-v${PUBLISH_VERSION}-arm64.exe',
			path: './bin/publish/win-arm64-lite/EdgePolicyManager.exe',
			contentType: 'application/octet-stream'
		}
	];

	function handleEnv(str) {
		return str.replace(/\$\{(.+?)}/g, (_, name) => {
			return process.env[name];
		});
	}

	// GitHub Release
	let releaseInfo;
	{
		const githubO = new Octokit({
			auth: GITHUB_TOKEN
		});

		const release = await githubO.repos.createRelease({
			owner, repo,
			tag_name: `v${PUBLISH_VERSION}`,
			name: CONFIG === 'Debug' ? `TestOnly ${PUBLISH_VERSION}` : `Release ${PUBLISH_VERSION}`,
			body: `## 更新日志

概述：**${COMMIT_TITLE}**

<details>

${COMMIT_BODY}

</details>

<hr>

> [!NOTE]
> 带有 RuntimeRequire 标记的版本可能需要在首次启动时自动下载并安装 .NET Runtime 和 Windows 应用 SDK。
>
> 任何 Runtime 都无需重复安装，这些 Runtime 由微软开发和维护。
>
> 如果你计划长期使用，建议选择此类版本，以便节省流量并提高效率。

> 发布时间：${PUBLISH_DATETIME}

> 策略版本：${EDGE_POLICY_VERSION}`,
			draft: CONFIG === 'Debug',
			prerelease: false
		});

		const releaseId = release.data.id;

		for (const file of files) {
			// 上传到upload_url
			await githubO.repos.uploadReleaseAsset({
				release_id: releaseId,
				owner, repo,
				name: handleEnv(file.name),
				data: fs.readFileSync(file.path)
			});
		}

		releaseInfo = await githubO.repos.getRelease({
			release_id: releaseId,
			owner, repo
		});
	}
	notice('GitHub Release 发布完成。');

	// Gitee Release
	{
		const giteeO = new Octokit({
			baseUrl: 'https://gitee.com/api/v5',
			auth: GITEE_TOKEN
		});

		function fileSizeString(size) {
			if (size < 1024) {
				return `${size} B`;
			} else if (size < 1024 * 1024) {
				return `${(size / 1024).toFixed(2)} KB`;
			} else if (size < 1024 * 1024 * 1024) {
				return `${(size / 1024 / 1024).toFixed(2)} MB`;
			} else if (size < 1024 * 1024 * 1024 * 1024) {
				return `${(size / 1024 / 1024 / 1024).toFixed(2)} GB`;
			} else {
				return `${(size / 1024 / 1024 / 1024 / 1024).toFixed(2)} TB`;
			}
		}

		let assetsTable = "|附件|大小|\n|---|---|\n",
			assetIcon = "![asset icon](data:image/svg+xml,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20height%3D%2216%22%20viewBox%3D%220%200%2016%2016%22%20width%3D%2216%22%3E%3Cpath%20d%3D%22m8.878.392%205.25%203.045c.54.314.872.89.872%201.514v6.098a1.75%201.75%200%200%201-.872%201.514l-5.25%203.045a1.75%201.75%200%200%201-1.756%200l-5.25-3.045A1.75%201.75%200%200%201%201%2011.049V4.951c0-.624.332-1.201.872-1.514L7.122.392a1.75%201.75%200%200%201%201.756%200ZM7.875%201.69l-4.63%202.685L8%207.133l4.755-2.758-4.63-2.685a.248.248%200%200%200-.25%200ZM2.5%205.677v5.372c0%20.09.047.171.125.216l4.625%202.683V8.432Zm6.25%208.271%204.625-2.683a.25.25%200%200%200%20.125-.216V5.677L8.75%208.432Z%22%3E%3C%2Fpath%3E%3C%2Fsvg%3E)";
		for (const asset of releaseInfo.data.assets) {
			assetsTable += `|${assetIcon} [${asset.name}](${asset.browser_download_url})|${fileSizeString(asset.size)}|\n`;
		}

		if (CONFIG === 'Debug') {
			await retry(() => giteeO.repos.getLatestRelease({owner, repo}));
		} else {
			await retry(() => giteeO.repos.createRelease({
				owner, repo,
				tag_name: `v${PUBLISH_VERSION}`,
				target_commitish: 'master',
				name: `发行版 ${PUBLISH_VERSION}`,
				body: `## 更新日志\n\n概述：**${COMMIT_TITLE}**\n\n<details>\n\n${COMMIT_BODY}\n\n</details>\n\n<hr>\n\n> 发布时间：${PUBLISH_DATETIME}\n\n> 策略版本：${EDGE_POLICY_VERSION}\n\n${assetsTable}`,
				prerelease: false
			}));
		}
	}
	notice('Gitee Release 发布完成。');
})();
