#!/bin/sh

host=pirg

help()
{
    echo "No Arguments given"
    echo "-a        Start all"
    echo "-main     Start main-service"
    echo "-docker   Start docker"
}

start_main()
{
    echo "Starting MainService"
    ssh $host "cd ./garden-os/ms && ./main-service &"
}

start_docker()
{
    echo "Starting Docker"
    ssh $host "cd ./garden-os && docker-compose up -d"
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
	*) 
        echo "Option $1 not recognized"
        read  -n 1 -p "Press any key to continue \n"
        exit 1 ;;
	esac
done

help
read  -n 1 -p "Press any key to continue \n"
exit 0 ;;