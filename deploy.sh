
path=/home/pi/garden-os/ms

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
scp ./main-service/bin/Debug/net7.0/linux-arm64/publish/* "$1:${path}"

echo Grand access 
ssh $1 "sudo chmod u+x ${path}/main-service"

echo Run Programm 
./start.sh -main

POSTGRES_PORT=5432
POSTGRES_USER=admin
POSTGRES_PASSWORD=SuperHighSecurePasswort
POSTGRES_DATABASE=main

PI_DB_HOST=localhost
PI_DB_PORT=5432
PI_DB_USER=admin
PI_DB_PASSWORD=SuperHighSecurePasswort
PI_DB_DATABASE=main