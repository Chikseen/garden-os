<template>
	<div class="hubLog">
		<HubControll />
		<LC v-if="isLogLoading" />
		<List v-else>
			<div v-for="(log, i) in logs" :key="i" class="grid_item grid_item_settings grid_item_text">
				<div class="grid_item_status">
					<h4>Status:</h4>
					<StatusIcon :status="log.status" />
					<h4>{{ log.status.charAt(0).toUpperCase() + log.status.slice(1) }}</h4>
				</div>
				<p>{{ log.message }}</p>
				<h4>Date: {{ formatTime(log.date) }}</h4>
				<!--	<h4>Triggerd by: {{ log.triggerd_by }}</h4> -->
			</div>
		</List>
	</div>
</template>

<script>
import LC from "@/components/ui/LoadingComponent.vue"
import List from "@/layout/ListLayout.vue";
import StatusIcon from "@/assets/StatusIcon.vue"

import { formatToDateTime } from "@/services/dates.js";

export default {
	components: {
		LC,
		List,
		StatusIcon,
	},
	data() {
		return {
			logs: null,
			isLogLoading: true
		}
	},
	methods: {
		formatTime(d) {
			return formatToDateTime(d)
		},
		async fetchHubLog() {
			this.isLogLoading = true
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}garden/${localStorage.getItem("selectedGarden")}/status/log`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			const j = await response.json()
			this.logs = j
			this.isLogLoading = false
		},
	},
	mounted() {
		this.fetchHubLog()
	},
}
</script>

<style lang="scss">
.hubLog {
	width: 100%;
}
</style>