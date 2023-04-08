#! /bin/bash

# Build Project in ARM
echo Build &&
dotnet publish --runtime linux-arm64 --self-contained  &&

# SSH on PI and clear deploy fodler
echo Clear && 
ssh pi@93.201.163.148 -p 8022 'rm /home/pi/deploy/*' &&

echo Send &&
# Copy files on PI
scp ./main-service/bin/Debug/net7.0/linux-arm64/publish/* pi:/home/pi/deploy &&

# Run
echo Run && 
ssh pi@93.201.163.148 -p 8022 'sudo chmod u+x /home/pi/deploy/main-service'
