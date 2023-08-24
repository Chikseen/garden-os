<template>
	<div v-if="data?.garden_info?.length > 0">
		<LC v-if="isLoading" />
		<div v-else class="info_wrapper" ref="infowrapper">
			<div class="info" v-for="(info, i) in data.garden_info" :key="i">
				<h3> {{ info.titel }} </h3>
				<p> {{ info.text }} </p>
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
		}
	},
	methods: {
		nextTile(counter) {
			setTimeout(() => {

				const elm = this.$refs.infowrapper
				const width = elm.offsetWidth
				const numberOfElemnts = this.data.garden_info.length

				elm.scrollTo({ top: 0, left: width * counter, behavior: "smooth" });

				if (counter >= (numberOfElemnts - 1))
					counter = -1

				this.nextTile(counter + 1)
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
				this.nextTile(1)

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
	height: calc(100% - 20px);
	padding: 10px;
	min-width: calc(100% - 20px);
	scroll-snap-align: center;

	&_wrapper {
		display: flex;
		flex-direction: row;
		overflow-x: scroll;
		scroll-snap-type: x mandatory;
		height: 100%;

		&::-webkit-scrollbar {
			display: none;
			-ms-overflow-style: none;
			scrollbar-width: none;
		}
	}

	h3,
	p {
		text-align: center;
	}
}
</style>