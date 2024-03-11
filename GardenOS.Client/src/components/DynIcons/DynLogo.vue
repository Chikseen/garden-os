<template>
	<SoilMoisture class="device_wrapper_icon" v-if="displayId == 'soil_moisture'" :value="value" />
	<UVIndex class="device_wrapper_icon" v-else-if="displayId == 'uv_index'" :value="value" />
	<Temperature class="device_wrapper_icon" v-else-if="displayId == 'temperature'" :value="value" />
	<div class="diagram_liniar" v-else>
		<div class="diagram_liniar_bar" :style="`width: ${value}%;`"></div>
	</div>
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