import json from "@/assets/static/userRoles.json";

function getRoleNameById(id) {
	return json[id];
}

export { getRoleNameById };
