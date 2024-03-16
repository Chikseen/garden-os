<template>
	<div v-if="sensorData" @click="this.$router.push(`device/${sensorData.deviceID}`)" class="device_wrapper">
		<DynLogo :displayId="sensorData.displayId" :value="sensorData.correctedValue" />
		<span v-if="isAddnewValueActive" class="device_wrapper_content" @click.stop="">
			<input type="number" v-model="newValue">
			<h1 class="device_wrapper_content_values_add" @click="submitNewValue">+</h1>
		</span>
		<span v-else class="device_wrapper_content">
			<span class="device_wrapper_content_values">
				<h2> {{ sensorData.name }} </h2>
				<h3> {{ sensorData.correctedValue?.toFixed(1) }} {{ sensorData.unit }} </h3>
				<h5> {{ timeLabel }} </h5>
			</span>
			<span v-if="sensorData.isManual" class="device_wrapper_content_values"
				@click.stop="isAddnewValueActive = !isAddnewValueActive">
				<h1 class="device_wrapper_content_values_add">+</h1>
			</span>
		</span>
	</div>
</template>

<script>
import DynLogo from "@/components/DynIcons/DynLogo.vue"

import { dynamicTimeDisplay } from "@/dates.js";
import { uploadNewValue } from "@/services/apiService.js"

export default {
	components: {
		DynLogo
	},
	props: {
		sensorData: { type: Object },
	},
	data() {
		return {
			timeLabel: null,
			timer: null,
			isAddnewValueActive: false,
			newValue: 0
		}
	},
	methods: {
		formatTime(d) {
			return dynamicTimeDisplay(d)
		},
		calcTime() {
			this.timeLabel = this.formatTime(this.sensorData.date)
			this.timer = setTimeout(() => {
				this.calcTime()
			}, 1000);
		},
		async submitNewValue() {
			await uploadNewValue(localStorage.getItem("selectedGarden"), this.sensorData.deviceId, this.sensorData.sensorId, this.newValue);
			this.isAddnewValueActive = false
		}
	},
	async mounted() {
		this.calcTime()
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
			justify-content: center;
			gap: 25px;

			&_values {
				display: flex;
				flex-direction: column;
				justify-content: space-evenly;

				&_add {
					background-color: rgb(132, 214, 143);
					padding: 15px;
					border-radius: 10px;
				}

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
}
</style>