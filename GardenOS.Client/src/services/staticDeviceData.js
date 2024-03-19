
function getColorByDevice(sensor) {
	console.log(sensor)

	switch (sensor.sensorTypeId) {
		case 0:
			return "#4cb269"
		case 1:
			return "#eab272"
		case 2:
			return "#3669c1"
		default:
			return "#99451a"
	}
}

export { getColorByDevice, };
