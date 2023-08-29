<template>
	<LC v-if="isHubStatusLoading"></LC>
	<div v-else class="grid_item_settings grid_item_text">
		<h3>Hub Controll</h3>
		<div class="grid_item_status">
			<h4>Status:</h4>
			<StatusIcon :status="deviceStatus?.status" />
			<h4>{{ deviceStatus?.status.charAt(0).toUpperCase() + deviceStatus?.status.slice(1) }}</h4>
		</div>
		<h5>{{ deviceStatus?.message }}</h5>
		<h6>{{ deviceStatus?.CurrentBuild }}</h6>
		<h6> Vor {{ formatTime(deviceStatus?.date) }} </h6>
		<ClickAndHoldButton v-if="showRebootButton && user.gardenData.userRole >= 20" @trigger="sendRebootRequest">
			Reboot (click & hold)
		</ClickAndHoldButton>
	</div>
</template>

<script>
import ClickAndHoldButton from "@/components/ClickAndHoldButton.vue"
import StatusIcon from "@/assets/StatusIcon.vue"
import LC from "@/components/ui/LoadingComponent.vue"

import { dynamicTimeDisplay } from "@/dates.js";
import { fetchUser } from "@/services/apiService.js"
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
			user: (state) => state.user,
		}),
	},
	methods: {
		formatTime(d) {
			return dynamicTimeDisplay(d)
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
	async mounted() {
		this.$store.commit("setUser", await fetchUser(localStorage.getItem("selectedGarden")))
		this.getHubStatus()
	}

}
</script>