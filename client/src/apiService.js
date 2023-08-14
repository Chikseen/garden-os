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
export { fetchGardenMeta, fetchDevices };
