<template>
	<div :class="['statusIcon', status == 'ok' ? 'statusIcon_ok' : '']"></div>
	<div :class="['statusIcon', status == 'reboot' ? 'statusIcon_reboot' : '']"></div>
	<div :class="['statusIcon', status == 'error' ? 'statusIcon_error' : '']"></div>
	<div :class="['statusIcon', status == 'noData' ? 'statusIcon_noData' : '']"></div>
	<div :class="['statusIcon', status == '' ? 'statusIcon_noData' : '']"></div>
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
	height: 10px;
	margin: auto 2px;
	border-radius: 50%;

	&_ok {
		background-color: #b0e95a;
	}

	&_reboot {
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