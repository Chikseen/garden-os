function formatToDateTime(payload) {
	const date = new Date(payload);
	return `${date.toLocaleDateString("de", { localeMatcher: "lookup", month: "numeric", day: "numeric", hour12: false })} ${date.toLocaleTimeString()}`;
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

function dynamicTimeDisplay(payload) {
	const date = new Date(`${payload}`);
	const now = new Date();
	const diff = (now - date) / 1000;
	if (diff < 3600) return `${(diff / 60).toFixed(0) <= 1 ? 'Jetzt' : 'Vor ' + (diff / 60).toFixed(0) + ' Minuten'}`;
	else if (diff < 86400) return `Vor ${(diff / 3600).toFixed(0) == 1 ? ' einer Stunde' : (diff / 3600).toFixed(0) + ' Stunden'}`;
	else if (diff < 604800) return `Vor ${(diff / 86400).toFixed(0) == 1 ? ' einen Tag' : (diff / 86400).toFixed(0) + ' Tagen'}`;
	return `Vor ${(diff / 604800).toFixed(0)} Wochen`;
}

export { formatToDateTime, toUTCISOString, dynamicTimeDisplay };
