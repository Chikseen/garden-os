async function fetchGardenMeta() {
  const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/garden`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${localStorage.getItem("apiToken")}`,
    },
  });
  const res = await initData.json();
  return res;
}
async function fetchDevices() {
  const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/datalog`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${localStorage.getItem("apiToken")}`,
    },
  })
  const res = await response.json()
  return res;
}

export { fetchGardenMeta, fetchDevices };