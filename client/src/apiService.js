async function fetchGardenMeta() {
	const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/garden`, {
		method: "GET",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
		},
	});
	const res = await initData.json();
	return res;
}
async function fetchDevices(gardenId) {
	const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/overview/${gardenId}`, {
		method: "GET",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
		},
	});
	const res = await response.json();
	return res;
}
export { fetchGardenMeta, fetchDevices };
