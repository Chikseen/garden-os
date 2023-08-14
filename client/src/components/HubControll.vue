<template>
	<LC v-if="isHubStatusLoading"></LC>
	<div v-else class="grid_item_settings grid_item_text">
		<h3>Hub Controll</h3>
		<div class="grid_item_status">
			<h4>Status:</h4>
			<StatusIcon :status="deviceStatus?.status" />
			<h4>{{ deviceStatus?.status }}</h4>
		</div>
		<h5>{{ formatTime(deviceStatus?.date) }} <!-- - Triggerd by: {{ deviceStatus?.triggerd_by }}--></h5>
		<ClickAndHoldButton v-if="showRebootButton" @trigger="sendRebootRequest">Reboot (click & hold)</ClickAndHoldButton>
	</div>
</template>

<script>
import ClickAndHoldButton from "@/components/ClickAndHoldButton.vue"
import StatusIcon from "@/assets/StatusIcon.vue"
import LC from "@/components/ui/LoadingComponent.vue"
import { formatToDateTime } from "@/dates.js";
import { mapState } from "vuex";


export default {
	components: {
		ClickAndHoldButton,
		StatusIcon,
		LC
	},
	props: {
		showRebootButton: { default: true }
	},
	data() {
		return {
			isHubStatusLoading: true,
		}
	},
	computed: {
		...mapState({
			deviceStatus: (state) => state.deviceStatus,
		}),
	},
	watch: {
		deviceStatus() {
			this.hub = this.deviceStatus
		}
	},
	methods: {
		formatTime(d) {
			return formatToDateTime(d)
		},
		async getHubStatus() {
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}garden/${localStorage.getItem("selectedGarden")}/status`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			const j = await response.json()
			this.$store.commit("setNewDeviceStatus", j)
			this.isHubStatusLoading = false
		},
		async sendRebootRequest() {
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}reboot/${this.deviceStatus?.rpi_id}`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
		},
	},
	mounted() {
		this.getHubStatus()
	}

}
</script>