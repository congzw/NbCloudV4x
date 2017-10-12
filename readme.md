# For NbCloud V4x

## 摘要

此项目用于nblcoud v4.x 

## 分支模型简介

分支模型可以分为两类：

- repository scoped(git)
- path scoped（tfvc）

非git版本管理一般是以文件夹结构来组织分支(subversions, sourcesafe, tfvc, ect.)，所以建议根文件以下列结构组织

	- Master(Trunk, Main, etc.)
		- /build
		- /doc
		- /lib
		- /setup
		- /src
		- /readme.md
	- Release1.0
	- Release2.0
	- Release2.1
	- ...

## 文件结构说明
- **build** 包含构建脚本和一些支持性文件(e.g., PowerShell scripts, EXEs, and the parameters file, etc.)
- **doc** 包含构文档(e.g., developer documents, installation guides, tips, requirements, etc.) 
- **lib** 包含第三方依赖库(e.g., nuget repos, ect.)
- **setup** 包含部署脚本或安装包代码(e.g.,  PowerShell, MSBuild, or NAnt script, WiX source code, etc.) 
- **src** 源代码(e.g.,  Visual Studio solution, all project folders, etc.)


## 项目结构

NbCloud.Common 常用工具库