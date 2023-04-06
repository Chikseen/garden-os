#! /bin/sh

dotnet publish --runtime linux-arm64 --self-contained  &&
scp -r .\main-service\bin\Debug\net7.0\linux-arm64\* pi:/home/pi/deploy &&
ssh pi &&
cd /home/pi/deploy
chmod +x main-service
./main-service