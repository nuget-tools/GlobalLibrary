#! /usr/bin/env bash
set -uvx
set -e
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
version="${ts}"
cd $cwd/GlobalLibrary.Json
dotnet run -c Release -f net462
dotnet run -c Release -f net6.0
cd $cwd/GlobalLibrary.Main
dotnet run -c Release -f net462
dotnet run -c Release -f net6.0
cd $cwd/GlobalLibrary.Script
dotnet run -c Release -f net462
dotnet run -c Release -f net6.0
cd $cwd/GlobalLibrary.Test
dotnet test -c Release -f net462
dotnet test -c Release -f net6.0
cd $cwd/Global
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" Global.csproj
rm -rf obj bin
java -jar ./antlr-4.12.0-complete.jar JSON5.g4 -Dlanguage=CSharp -package Global.Parser.Json5 -o Parser/Json5
rm -rf *.nupkg
dotnet pack -c Rlease -o .
#exit 0
cd $cwd
git add .
git commit -m"GlobalLibrary v$version"
git tag -a v$ts -mv$version
git push origin v$version
git push
git remote -v
