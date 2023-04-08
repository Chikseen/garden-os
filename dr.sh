#! /bin/bash

path=/home/pi/garden-os/ms
host=93.201.163.148
port=8022

# Build Project in ARM
echo Build &&
dotnet publish --runtime linux-arm64 --self-contained  &&

# SSH on PI and clear deploy fodler
echo Clear && 
ssh pi@$host -p $port "rm ${path}/*" &&

echo Send &&
# Copy files on PI
scp ./main-service/bin/Debug/net7.0/linux-arm64/publish/* pi:${path} &&

# Run
echo Run && 
ssh pi@$host -p $port "sudo chmod u+x ${path}/main-service"

# read  -n 1 -p "Magic Debugger(Thx windows)"