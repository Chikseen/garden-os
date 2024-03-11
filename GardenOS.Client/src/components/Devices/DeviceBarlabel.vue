<template>
	<div v-if="sensorData" @click="this.$router.push(`device/${sensor.deviceId}`)" class="device_wrapper">
		<LC v-if="isLoading" />
		<DynLogo v-else :displayId="sensorData.displayId" :value="sensorData.correctedValue" />
		<span class="device_wrapper_content">
			<h2> {{ sensor.name }} </h2>
			<h3> {{ sensorData.correctedValue?.toFixed(1) }} {{ sensor.unit }} </h3>
			<h5> {{ timeLabel }} </h5>
		</span>
	</div>
</template>

<script>
import DynLogo from "@/components/DynIcons/DynLogo.vue"
import LC from "@/components/ui/LoadingComponent.vue"

import { dynamicTimeDisplay } from "@/dates.js";
import { fetchDeviceSensorData } from "@/services/apiService.js"

export default {
	components: {
		DynLogo,
		LC
	},
	props: {
		sensor: { type: Object },
	},
	data() {
		return {
			sensorData: null,
			isLoading: true,
			timeLabel: null,
			timer: null,
		}
	},
	methods: {
		formatTime(d) {
			return dynamicTimeDisplay(d)
		},
		calcTime() {
			this.timeLabel = this.formatTime(this.device.date)
			this.timer = setTimeout(() => {
				this.calcTime()
			}, 1000);
		}
	},
	async mounted() {
		//this.calcTime()
		this.sensorData = await fetchDeviceSensorData(localStorage.getItem("selectedGarden"), this.sensor.deviceId, this.sensor.sensorId);
		this.isLoading = false;
	},
	beforeUnmount() {
		clearTimeout(this.timer)
	}
}
</script>

<style lang="scss">
.device {
	&_wrapper {
		cursor: pointer;
		display: flex;
		flex-direction: column;
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