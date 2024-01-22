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

cd $cwd
echo "# GlobalLibrary" > README.md
echo "" >> README.md
echo "\`\`\`" >> README.md
iconv -f cp932 -t utf-8 GlobalLibrary.Main/Program.cs >> README.md
echo "\`\`\`" >> README.md

echo "" >> README.md

echo "# JavaScript" >> README.md
echo "" >> README.md
echo "\`\`\`" >> README.md
iconv -f cp932 -t utf-8 GlobalLibrary.Script//Program.cs >> README.md
echo "\`\`\`" >> README.md

echo "" >> README.md

echo "# JSON" >> README.md
echo "" >> README.md
echo "\`\`\`" >> README.md
iconv -f cp932 -t utf-8 GlobalLibrary.json//Program.cs >> README.md
echo "\`\`\`" >> README.md

cd $cwd/Global
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" Global.csproj
rm -rf obj bin
java -jar ./antlr-4.13.1-complete.jar JSON5.g4 -Dlanguage=CSharp -package Global.Parser.Json5 -o Parser/Json5
rm -rf *.nupkg
dotnet pack -o . -p:Configuration=Release -p:Platform="Any CPU"

#exit 0

cd $cwd
git add .
git commit -m"GlobalLibrary v$version"
git tag -a v$ts -mv$version
git push origin v$version
git push
git remote -v
