<template>
	<div class="gardenOverview_wrapper">
		<h1>Garden</h1>
		<LC v-if="isGardenListLoading" />
		<DynamicGrid v-else>
			<div v-for="garden in gardenList?.garden_data" :key="garden.garden_id" class="grid_item item"
				@click="setGarden(garden.garden_id)">
				<h2> {{ garden.garden_name }} </h2>
				<h5> {{ garden.garden_id }} </h5>
				<h5> {{ garden.weather_location }} </h5>
			</div>
			<div class="grid_item item item_input">
				<input type="text" name="" id="" placeholder="gardenID" @change="(e) => newGardenId = e.target.value">
				<button @click="sendAccessRequest()">Request Access</button>
			</div>

		</DynamicGrid>

		<hr>
		
		<h1>User Settings</h1>
		<div v-if="selectedGarden">
			<LC v-if="isUserListLoading" />
			<span v-else>
				<h2>Approved User</h2>
				<DynamicGrid>
					<div v-for="user in userList?.userList.filter(u => u.isApproved)" :key="user" class="grid_item item">
						<h2 @click="toggleUserStatus(user)"> {{ user.user_id }} </h2>
					</div>
				</DynamicGrid>

				<h2>Requests to join the garden</h2>
				<DynamicGrid>
					<div v-for="user in userList?.userList.filter(u => !u.isApproved)" :key="user" class="grid_item item">
						<h2 @click="toggleUserStatus(user)"> {{ user.user_id }} </h2>
					</div>
				</DynamicGrid>
			</span>
		</div>
		<p v-else>No Garden Selected, please select a garden to see current user</p>
	</div>
</template>

<script>
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"

import { fetchGardenMeta } from "@/apiService.js"
import { mapState } from "vuex";

export default {
	components: {
		DynamicGrid,
		LC,
	},
	data() {
		return {
			selectedGarden: null,
			userList: null,
			newGardenId: null,
			isGardenListLoading: true,
			isUserListLoading: true,
		}
	},
	methods: {
		setGarden(id) {
			localStorage.setItem('selectedGarden', id)
			this.$router.push("/overview")
		},
		async fetchuser() {
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/users/${this.selectedGarden}`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});

			this.userList = await response.json();

			this.isUserListLoading = false;
		},
		async sendAccessRequest() {
			if (this.newGardenId) {
				const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/accessrequest/${this.newGardenId}`, {
					method: "GET",
					headers: {
						Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					},
				});
			}
		},
		async toggleUserStatus(user) {
			this.isUserListLoading = true;
			console.log(user)
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/changestatus/${user.garden_id}/${user.user_id}`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			await this.fetchuser()
		},
	},
	computed: {
		...mapState({
			gardenList: (state) => state.gardenList,
		}),
	},
	async mounted() {
		this.isGardenListLoading = true
		this.selectedGarden = localStorage.getItem('selectedGarden')
		this.$store.commit("setGardenList", await fetchGardenMeta())
		if (this.selectedGarden) {
			this.fetchuser()
		}
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
	padding: 10px 0;

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
		margin: auto;
		padding: 15px;
	}
}
</style>