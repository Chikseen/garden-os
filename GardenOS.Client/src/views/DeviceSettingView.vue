<template>
	<div class="deviceSetting_wrapper">
		<h1>Device Settings</h1>
		<LC v-if="isLoading" />
		<div v-else>
			<Grid>
				<div v-for="device in deviceMeta" :key="device.id" class="grid_item item deviceSetting_list_item">
					<span>
						<input type="text" v-model="device.name" style="font-size: 1.5rem;" />
						<h4>{{ device.id }}</h4>
					</span>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Display Id</label>
							<h6>The display id will define the logo</h6>
						</span>
						<select v-model="device.displayId">
							<option value="soil_moisture">Soil Moisture</option>
							<option value="uv_index">UV Index</option>
							<option value="temperature">Temperature</option>
							<option value="">None</option>
						</select>
						<div v-if="device.displayId" class="deviceSetting_list_item_row">
							<input v-if="device.displayId == 'soil_moisture'" type="range" min="0" max="130"
								v-model="device.value">
							<input v-if="device.displayId == 'uv_index'" type="range" min="0" max="11"
								v-model="device.value">
							<input v-if="device.displayId == 'temperature'" type="range" min="-5" max="30"
								v-model="device.value">
						</div>
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Sort Order</label>
							<h6>Lower Number will be show before devices with a higher Number</h6>
						</span>
						<input type="number" min="0" max="100" v-model="device.sortOrder">
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Is Manual</label>
						</span>
						<p>{{ device.isManual }}</p>
					</div>
					<!--SENSOR-->
					<div v-for="sensor in deviceSensorMeta[device.id]" :key="sensor.sensorId">
						<span>
							<input type="text" style="font-size: 1.5rem;" :placeholder="sensor.name" />
							<h4>{{ sensor?.sensorId }}</h4>
						</span>
						<div class="deviceSetting_list_item_row">
							<span>
								<label>Upper Limit</label>
							</span>
							<input type="number" min="0" max="100" v-model="sensor.upperLimit">
						</div>
						<div class="deviceSetting_list_item_row">
							<span>
								<label>Lower Limit</label>
							</span>
							<input type="number" min="0" max="100" v-model="sensor.lowerLimit">
						</div>
						<div class="deviceSetting_list_item_row">
							<span>
								<label>Sensor Type Id </label>
							</span>
							<input type="number" min="0" max="2" v-model="sensor.sensorTypeId">
						</div>
						<div class="deviceSetting_list_item_row">
							<span>
								<label>Unit</label>
							</span>
							<input type="text" v-model="sensor.unit">
						</div>
						<div class="deviceSetting_list_item_row">
							<span>
								<label>Unit</label>
							</span>
							<p>{{ sensor.isManual }}</p>
						</div>
					</div>
					<!--ADD SENSOR-->
					<div class="grid_item item deviceSetting_list_item">
						<h1>Add "{{ device.name }}" Sensor</h1>
						<span>
							<label for="newDeviceIsManual">Unit</label>
							<input type="text" v-model="newSensor.Unit">
						</span>
						<span>
							<label for="newDeviceName">Name</label>
							<input id="newDeviceName" type="text" v-model="newSensor.Name">
						</span>
						<span>
							<label for="newDeviceName">SensorTypeId</label>
							<input id="newDeviceName" type="number" min="-1" max="2" v-model="newSensor.SensorTypeId">
						</span>
						<span>
							<label for="newDeviceName">Upper Limit</label>
							<input id="newDeviceName" type="number" v-model="newSensor.UpperLimit">
						</span>
						<span>
							<label for="newDeviceName">Lower Limit</label>
							<input id="newDeviceName" type="number" v-model="newSensor.LowerLimit">
						</span>
						<h1 @click="createNewSensor(device)">+</h1>
					</div>
				</div>
				<!--ADD DEVICE-->
				<div class="grid_item item deviceSetting_list_item">
					<h1>Add Device</h1>
					<span>
						<label for="newDeviceIsManual">Is Manual</label>
						<input id="newDeviceIsManual" type="checkbox" v-model="newDeviceIsManual">
					</span>
					<span>
						<label for="newDeviceName">Name</label>
						<input id="newDeviceName" type="text" v-model="newDeviceName">
					</span>
					<h1 @click="creatNewDevice">+</h1>
				</div>
			</Grid>
		</div>
	</div>
</template>

<script>
import Grid from "@/layout/DynamicGridLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"
import DynLogo from "@/components/DynIcons/DynLogo.vue"

import { fetchDeviceMeta, fetchDeviceSensorMeta } from "@/services/apiService.js"
import { mapState } from "vuex";

export default {
	components: {
		Grid,
		LC,
		DynLogo,
	},
	data() {
		return {
			deviceMeta: null,
			deviceSensorMeta: {},
			isLoading: true,
			newDeviceIsManual: "",
			newDeviceName: "",
			newSensor: {
				DeviceId: "",
				Name: "",
				Unit: "",
				UpperLimit: 100,
				LowerLimit: 0
			}
		}
	},
	methods: {
		async saveDevice(device) {
			await fetch(`${process.env.VUE_APP_PI_HOST}garden/changedevice`, {
				method: "PATCH",
				body: JSON.stringify({
					"id": device.id,
					"name": device.name,
					"displayId": device.displayId,
					"upper_limit": device.upper_limit,
					"lower_limit": device.lower_limit,
					"sortOrder": device.sortOrder,
					"groupId": device.groupId,
					"unit": device.unit,
				}),
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
			});
		},
		async creatNewDevice() {
			await fetch(`${process.env.VUE_APP_PI_HOST}devices/create`, {
				method: "POST",
				body: JSON.stringify({
					"IsManual": this.newDeviceIsManual,
					"Name": this.newDeviceName,
					"GardenId": this.gardenMeta.garden_id,
				}),
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
			});
		},
		async createNewSensor(device) {
			this.newSensor.DeviceId = device.id

			await fetch(`${process.env.VUE_APP_PI_HOST}devices/create/sensor`, {
				method: "POST",
				body: JSON.stringify(this.newSensor),
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
			});
			await this.fetchDevices()
		},
		async fetchDevices() {
			this.deviceMeta = await fetchDeviceMeta(localStorage.getItem("selectedGarden"))
			this.deviceMeta.forEach(async device => {
				let sensorData = await fetchDeviceSensorMeta(localStorage.getItem("selectedGarden"), device.id)
				this.deviceSensorMeta[device.id] = sensorData;
			});
		}
	},
	computed: {
		...mapState({
			gardenMeta: (state) => state.gardenMeta,
		}),
	},
	async mounted() {
		await this.fetchDevices()
		this.isLoading = false
	}
} 
</script>

<style lang="scss">
.deviceSetting {
	&_wrapper {
		max-width: 1750px;
		width: 100%;
		margin: 15px auto;

		h1,
		p {
			padding-left: 15px;
		}
	}

	&_list {

		&_item {
			grid-column-start: span 2;
			display: flex;
			flex-direction: column;
			justify-content: start;
			gap: 15px;

			&_row {
				display: flex;
				gap: 15px;
			}

			span {

				h2,
				h4 {
					text-align: left;
				}
			}
		}
	}
}
</style>