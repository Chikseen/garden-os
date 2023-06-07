function formatToDateTime(payload) {
  const date = new Date(payload);
  return `${date.toLocaleDateString("ger", { weekday: "long", year: "numeric", month: "numeric", day: "numeric" })} ${date.toLocaleTimeString()};`
}

export { formatToDateTime };
