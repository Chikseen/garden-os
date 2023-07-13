<template>
	<div class="StatusIcon"></div>
</template>

<script>
export default {
	props: {
		status: { type: String, default: "ok" }
	}
}
</script>

<style lang="scss">
.StatusIcon {
	top: 0;
	left: 0;
	width: 10px;
	height: 10px;
	margin: auto 10px;
	border-radius: 50%;
	background-color: black;
}
</style>