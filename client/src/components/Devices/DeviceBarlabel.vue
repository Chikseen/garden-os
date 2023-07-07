<template>
	<div @click="this.$router.push(`device/${device.device_id}`)" class="device_wrapper">
		<h2> {{ device.name }} </h2>
		<h3> {{ device.corrected_value.toFixed(1) }} {{ device.unit }} </h3>
		<div class="diagram_liniar">
			<div class="diagram_liniar_bar" :style="`width: ${device.corrected_value}%;`"></div>
		</div>
		<h5> {{ formatTime(device.date) }} </h5>
	</div>
</template>

<script>
import { formatToDateTime } from "@/dates.js";

export default {
	props: {
		device: { type: Object }
	},
	methods: {
		formatTime(d) {
			return formatToDateTime(d)
		}
	},
}
</script>

<style lang="scss">
.device {
	&_wrapper {
		cursor: pointer;
		display: flex;
		flex-direction: column;
		justify-content: space-evenly;
		background-color: #ffffff;
		width: auto;
		padding: 10px 0;

		h1,
		h2,
		h3,
		h5,
		p {
			text-align: center;
		}
	}
}
</style>