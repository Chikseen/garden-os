<template>
	<div @click="this.$router.push(`device/${device.device_id}`)" class="device_wrapper">
		<DynLogo :displayId="device.display_id" :value="device.corrected_value" />
		<span class="device_wrapper_content" :style="device.display_id === '' ? 'width: 100%;' : ''">
			<h2> {{ device.name }} </h2>
			<h3> {{ device.corrected_value.toFixed(1) }} {{ device.unit }} </h3>
			<div class="diagram_liniar" v-if="device.display_id === ''">
				<div class="diagram_liniar_bar" :style="`width: ${device.corrected_value}%;`"></div>
			</div>
			<h5> {{ formatTime(device.date) }} </h5>
		</span>
	</div>
</template>

<script>
import DynLogo from "@/components/DynIcons/DynLogo.vue"

import { formatToDateTime } from "@/dates.js";

export default {
	components: {
		DynLogo
	},
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
		flex-direction: row;
		justify-content: center;
		gap: 15px;
		padding: 10px 5px;
		background-color: #ffffff;

		&_icon {
			margin: auto 0;
		}

		&_content {
			display: flex;
			flex-direction: column;
			justify-content: space-evenly;

			h1,
			h2,
			h3,
			h5,
			p {
				text-align: center;
			}
		}

	}
}
</style>