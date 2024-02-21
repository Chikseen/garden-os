#!/bin/sh

host=piL

help()
{
    echo "No Arguments given"
    echo "-a        Stop all"
    echo "-main     Stop rpi-data"
    echo "-docker   Stop docker"
}

stop_main()
{
    echo "Stoping MainService"
    ssh $host "pkill -f rpi-data"
}

stop_docker()
{
    echo "Stoping Docker"
    ssh $host "cd ./garden-os && docker-compose down"
}

while [ -n "$1" ]; do
	case "$1" in
	-a) 
        stop_main $1
        stop_docker $1
        #read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
	-main)
        stop_main $1
        #read  -n 1 -p "Press any key to continue \n"
        exit 0 ;;
    -docker) 
        stop_docker $1 
        #read  -n 1 -p "Press any key to continue \n"
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