#!/bin/sh

host=piL

help()
{
    echo "No Arguments given"
    echo "-a        Start all"
    echo "-main     Start rpi-data"
    echo "-docker   Start docker"
}

start_main()
{
    echo "Starting MainService"
    ssh $host "pkill -f rpi-data"
    ssh $host "cd ./garden-os/rpi-data && ./rpi-data &"
}

start_docker()
{
    echo "Starting Docker"
    ssh $host "cd ./garden-os && docker-compose up -d"
}

start_docker-build()
{
    echo "Starting Docker"
    ssh $host "cd ./garden-os && docker-compose up -d --build"
}

while [ -n "$1" ]; do
	case "$1" in
	-a) 
        start_docker $1
        start_main $1
        read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
	-main)
        start_main $1
        read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
    -docker) 
        start_docker $1 
        read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
    -docker-build) 
        start_docker-build $1 
        read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
	*) 
        echo "Option $1 not recognized"
        read  -n 1 -p "Press any key to continue \n"
        exit 1 ;;
	esac
done

help
read  -n 1 -p "Press any key to continue \n"
exit 0 ;;