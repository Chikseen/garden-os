<template>
	<div class="gardenOverview_wrapper">
		<h1>Garden</h1>
		<LC v-if="isGardenListLoading" />
		<List v-else>
			<div v-for="garden in gardenList?.garden_data" :key="garden.garden_id" class="list_item item"
				@click="setGarden(garden.garden_id)">
				<h2> {{ garden.garden_name }} </h2>
				<h5> {{ garden.garden_id }} </h5>
				<h5> {{ garden.weather_location }} </h5>
			</div>
			<div class="list_item item item_input">
				<input type="text" name="" id="" placeholder="gardenID" @change="(e) => newGardenId = e.target.value"
					:value="newGardenId">
				<button @click="sendAccessRequest()">
					<LC v-if="isAccessRequestLoading" />
					<p v-else>Request Access</p>
				</button>
				<button @click="logout">LogoutD</button>
			</div>

		</List>

		<h1>Pending requests</h1>
		<LC v-if="isAccessRequestLoading" />
		<List v-else>
			<div v-for="req in requestedGarden" :key="req" class="list_item item">
				<h2>{{ req }}</h2>
			</div>
		</List>
	</div>
</template>

<script>
import List from "@/layout/ListLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"

import { mapState, mapActions } from "vuex";

export default {
	components: {
		List,
		LC,
	},
	data() {
		return {
			newGardenId: null,
			isGardenListLoading: true,
			isAccessRequestLoading: false,
			requestedGarden: [],
		}
	},
	methods: {
		setGarden(id) {
			localStorage.setItem('selectedGarden', id)
			this.$router.push("/overview")
		},
		async sendAccessRequest() {
			this.isAccessRequestLoading = true
			if (this.newGardenId) {
				const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/${this.newGardenId}/accessrequest`, {
					method: "GET",
					headers: {
						Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					},
				});
			}
			this.loadRequested();
			this.newGardenId = ""
		},
		async loadRequested() {
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/requested`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			this.requestedGarden = await response.json()

			this.isAccessRequestLoading = false
		},
		...mapActions({
			logout: 'logout',
			fetchGardenMeta: 'fetchGardenMeta'
		})
	},
	computed: {
		...mapState({
			gardenList: (state) => state.gardenList,
			keycloak: (state) => state.keycloak,
		}),
	},
	async mounted() {
		this.isGardenListLoading = true
		this.selectedGarden = localStorage.getItem('selectedGarden')
		this.fetchGardenMeta()
		await this.loadRequested();
		this.isGardenListLoading = false
	}
}
</script>

<style lang="scss">
.item {
	cursor: pointer;
	display: flex;
	flex-direction: column;
	justify-content: space-evenly;
	background-color: #ffffff;
	width: auto;
	padding: 10px;

	h1,
	h2,
	h3,
	h5,
	p {
		text-align: center;
	}

	&_input {
		padding: 15px;
		display: flex;
		flex-direction: column;
		gap: 15px;
	}
}

.gardenOverview {
	&_wrapper {
		max-width: 750px;
		width: 100%;
		margin: 0 auto;
		padding: 15px 0;

		h1,
		h2,
		p {
			padding-left: 15px;
		}
	}
}
</style>services/apiService.js