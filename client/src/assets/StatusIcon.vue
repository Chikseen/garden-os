<template>
	<div :class="`statusIcon statusIcon_${status}`"></div>
</template>

<script>
export default {
	props: {
		status: { type: String, default: "noData" }
	}
}
</script>

<style lang="scss">
.statusIcon {
	top: 0;
	left: 0;
	width: 10px;
	min-width: 10px;
	height: 10px;
	margin: auto 5px;
	border-radius: 50%;

	&_ok {
		background-color: #b0e95a;
	}

	&_warning {
		background-color: #e9e05a;
	}

	&_error {
		background-color: #e95a5a;
	}
	&_noData {
		background-color: #c0c0c0;
	}
}
</style>