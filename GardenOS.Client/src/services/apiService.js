async function fetchGardenMeta() {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}user/garden`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function fetchDevices(gardenId) {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}garden/${gardenId}/overview`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function fetchDeviceMeta(gardenId) {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}devices/${gardenId}`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function fetchDeviceSensorLatestValues(gardenId, deviceId) {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}devices/${gardenId}/${deviceId}/0`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function fetchDeviceSensorMeta(gardenId, deviceId) {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}devices/sensor/${deviceId}`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function uploadNewValue(gardenId, deviceId, sesnorId, payload) {
	let response;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}devices/manual`, {
			method: "POST",
			body: JSON.stringify(
				{
					GardenId: gardenId,
					DeviceId: deviceId,
					SensorId: sesnorId,
					Value: payload * 1,
				}),
			headers: {
				'Authorization': `Bearer ${localStorage.getItem("accessToken")}`,
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}

async function fetchUser(gardenId) {
	let response;
	if (gardenId == null || gardenId == undefined || gardenId.length == 0) return null;
	if (localStorage.getItem("accessToken")) {
		response = await fetch(`${process.env.VUE_APP_PI_HOST}user/${gardenId}`, {
			method: "GET",
			headers: {
				"Cache-Control": "no-cache",
				Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
			},
		});
	}

	if (response?.status > 200) {
		return null;
	}

	const res = await response.json();
	return res;
}
export {
	fetchGardenMeta,
	fetchDevices,
	fetchUser,
	fetchDeviceMeta,
	fetchDeviceSensorLatestValues,
	fetchDeviceSensorMeta,
	uploadNewValue
};
