async function fetchGardenMeta() {
	const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("userName")}/garden`, {
		method: "GET",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("apiToken")}`,
		},
	});
	const res = await initData.json();
	return res;
}
async function fetchDevices() {
	const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("userName")}/datalog`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
		},
	});
	const res = await response.json();
	return res;
}
export { fetchGardenMeta, fetchDevices };
