<template>
	<div class="deviceSetting_wrapper">
		<h1>Device Settings</h1>
		<LC v-if="isLoading" />
		<div v-else>
			<Grid>
				<div v-for="device in deviceData?.devices" :key="device.device_id"
					class="grid_item item deviceSetting_list_item">
					<span>
						<input type="text" v-model="device.name" style="font-size: 1.5rem;" />
						<h4>{{ device.device_id }}</h4>
					</span>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Current Value: </label>
							<p>{{ (device.value * 1).toFixed(1) }} </p>
						</span>
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Display Id</label>
							<h6>The display id will define the logo</h6>
						</span>
						<select v-model="device.display_id">
							<option value="soil_moisture">Soil Moisture</option>
							<option value="uv_index">UV Index</option>
							<option value="temperature">Temperature</option>
							<option value="">None</option>
						</select>
						<div v-if="device.display_id" class="deviceSetting_list_item_row">
							<DynLogo :displayId="device.display_id" :value="device.value" />
							<input v-if="device.display_id == 'soil_moisture'" type="range" min="0" max="130"
								v-model="device.value">
							<input v-if="device.display_id == 'uv_index'" type="range" min="0" max="11"
								v-model="device.value">
							<input v-if="device.display_id == 'temperature'" type="range" min="-5" max="30"
								v-model="device.value">
						</div>
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Upper Limit</label>
							<h6>Defines the Upper Limit on which the value will be adjusted</h6>
						</span>
						<input type="number" min="0" max="100" v-model="device.upper_limit">
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Lower Limit</label>
							<h6>Defines the Lower Limit on which the value will be adjusted</h6>
						</span>
						<input type="number" min="0" max="100" v-model="device.lower_limit">
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Sort Order</label>
							<h6>Lower Number will be show before devices with a higher Number</h6>
						</span>
						<input type="number" min="0" max="100" v-model="device.sort_order">
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Group Id</label>
							<h6>Devices with the same Group Id will be grouped in one box</h6>
						</span>
						<input type="text" v-model="device.group_id">
					</div>
					<div class="deviceSetting_list_item_row">
						<span>
							<label>Unit</label>
						</span>
						<input type="text" v-model="device.unit">
					</div>
					<div class="deviceSetting_list_item_row">
						<button @click="saveDevice(device)">Save</button>
						<button @click="fetchDevices">Load Original</button>
					</div>
				</div>
			</Grid>
		</div>
	</div>
</template>

<script>
import Grid from "@/layout/DynamicGridLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"
import DynLogo from "@/components/DynIcons/DynLogo.vue"

import { mapState, mapActions } from "vuex";

export default {
	components: {
		Grid,
		LC,
		DynLogo,
	},
	data() {
		return {
			isLoading: true
		}
	},
	methods: {
		async saveDevice(device) {
			await fetch(`${process.env.VUE_APP_PI_HOST}garden/changedevice`, {
				method: "PATCH",
				body: JSON.stringify({
					"device_id": device.device_id,
					"name": device.name,
					"display_id": device.display_id,
					"upper_limit": device.upper_limit,
					"lower_limit": device.lower_limit,
					"sort_order": device.sort_order,
					"group_id": device.group_id,
					"unit": device.unit,
				}),
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
			});
		},
		...mapActions({
			fetchDevices: 'fetchDevices'
		})
	},
	computed: {
		...mapState({
			deviceData: (state) => state.deviceData,
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