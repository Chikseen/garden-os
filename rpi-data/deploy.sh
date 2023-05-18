
path=/home/pi/garden-os/rpi-data

if [ -z "$1" ]
then
    echo "No SSH host specified"
    read -n 1 -p "Press any key to continue \n"
    exit 1
fi


echo Build
dotnet publish --runtime linux-arm64 --self-contained

echo Remove old data
ssh $1 "rm ${path}/*"

echo Send Programm to RPI
scp ./bin/Debug/net7.0/linux-arm64/publish/* "$1:${path}"

echo Grand access 
ssh $1 "sudo chmod u+x ${path}"
    read -n 1 -p "Press any key to continue \n"
echo Run Programm 
./start.sh -main