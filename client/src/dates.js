function formatToDateTime(payload) {
	const date = new Date(payload);
	return `${date.toLocaleDateString("de", { localeMatcher: "lookup",  month: "numeric", day: "numeric", hour12: false })} ${date.toLocaleTimeString()};`;
}

function toUTCISOString(date) {
	var tzo = -date.getTimezoneOffset(),
		dif = tzo >= 0 ? "+" : "-",
		pad = function (num) {
			return (num < 10 ? "0" : "") + num;
		};

	return (
		date.getFullYear() +
		"-" +
		pad(date.getMonth() + 1) +
		"-" +
		pad(date.getDate()) +
		"T" +
		pad(date.getHours()) +
		":" +
		pad(date.getMinutes()) +
		":" +
		pad(date.getSeconds()) +
		dif +
		pad(Math.floor(Math.abs(tzo) / 60)) +
		":" +
		pad(Math.abs(tzo) % 60)
	);
}

export { formatToDateTime, toUTCISOString };
