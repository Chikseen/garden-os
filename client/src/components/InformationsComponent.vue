<template>
	<div v-if="data?.garden_info?.length > 0">
		<LC v-if="isLoading" />
		<div v-else>
			<div class="info">
				<h2> {{ data.garden_info[currentTile]?.titel }} </h2>
				<p> {{ data.garden_info[currentTile]?.text }} </p>
			</div>
		</div>
	</div>
</template>

<script>
import LC from "@/components/ui/LoadingComponent.vue"

export default {
	components: {
		LC
	},
	data() {
		return {
			isLoading: true,
			data: null,
			currentTile: 0,
		}
	},
	methods: {
		nextTile() {
			setTimeout(() => {
				this.currentTile++;
				if (this.data.garden_info.length <= this.currentTile)
					this.currentTile = 0;
				this.nextTile()
			}, 5000);
		},
		async load() {
			this.isLoading = true
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}garden/${localStorage.getItem("selectedGarden")}/info`, {
				method: "GET",
				headers: {
					'Authorization': `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			this.data = await response.json();

			if (this.data?.garden_info.length > 1)
				this.nextTile()

			this.isLoading = false
		}
	},
	mounted() {
		this.load()
	},
}
</script>

<style lang="scss">
.info {
	display: flex;
	flex-direction: column;
	justify-content: center;
	gap: 10px;
	height: 100%;

	h2,
	p {
		text-align: center;
	}
}
</style>