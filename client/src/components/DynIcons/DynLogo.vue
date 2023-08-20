<template>
	<SoilMoisture class="device_wrapper_icon" v-if="displayId == 'soil_moisture'" :value="value" />
	<UVIndex class="device_wrapper_icon" v-if="displayId == 'uv_index'" :value="value" />
	<Temperature class="device_wrapper_icon" v-if="displayId == 'temperature'" :value="value" />
</template>

<script>
import SoilMoisture from "@/components/DynIcons/SoilMoisture"
import UVIndex from "@/components/DynIcons/UVIndex"
import Temperature from "@/components/DynIcons/Temperature"

export default {
	components: {
		SoilMoisture,
		UVIndex,
		Temperature,
	},
	props: {
		displayId: { type: String },
		value: { type: Number },
	}
}
</script>