#! /usr/bin/env bash
set -uvx
set -e
gh auth login --hostname github.com --git-protocol https --web
userName=nuget-tools
appName=GlobalLibrary.Main
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
version=${ts}
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" ${appName}.csproj
rm -rf obj bin ${appName}.*.zip
dotnet build -c Release ${appName}.csproj
cd bin/x64/Release/net462
#rm -rf *.pdb
7z a -r -tzip $cwd/${appName}.v$ts.zip *
cd $cwd
gh release create --repo ${userName}/tools --generate-notes windows-64bit || true
url="https://github.com/${userName}/tools/releases/download/windows-64bit/${appName}.v${ts}.zip"
gh release upload windows-64bit "${appName}.v${ts}.zip" --repo ${userName}/tools --clobber
cat << EOS > ${appName}.xml
<?xml version="1.0" encoding="utf-8"?>
<software>
  <version>v${ts}</version>
  <url>$url</url>
</software>
EOS
gh release upload windows-64bit "${appName}.xml" --repo ${userName}/tools --clobber
