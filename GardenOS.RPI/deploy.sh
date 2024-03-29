
srcPath=./bin/Release/net7.0/linux-arm64/publish
targetPath=../rpi-binary

json= cat ${targetPath}/build.json
json=`cat ${targetPath}/build.json`
buildnumber=`grep -o '[0-9]*' <<< $json`
buildnumber=$((buildnumber+1))

dotnet publish --runtime linux-arm64 --self-contained -c Release
if [ $? -eq 0 ] 
then
	echo Clean 
	rm -r ${targetPath}/*

	echo Build
	pwd

  	echo Copy
	cp -r $srcPath/rpi-data $targetPath

	echo Build number $buildnumber
	echo "
	{
		\"build\": ${buildnumber}
	}" | tee ${targetPath}/build.json

	echo "
	{
		\"build\": ${buildnumber}
	}" | tee ../main-service/build.json
		read -n 1 -p "Press any key to continue \n"

else
    echo "build failed"
    read -n 1 -p "Press any key to continue \n"
    exit 1
fi