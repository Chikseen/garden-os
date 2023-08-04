<template>
	<div class="userOverview_wrapper">
		<h1>User Settings</h1>
		<LC v-if="isUserListLoading" />
		<span v-else>
			<h2>Approved User</h2>
			<List>
				<div v-for="user in userList?.userList.filter(u => u.isApproved)" :key="user" class="grid_item item">
					<h2 @click="toggleUserStatus(user)"> {{ user.user_id }} </h2>
				</div>
			</List>

			<h2>Requests to join the garden</h2>
			<List>
				<div v-for="user in userList?.userList.filter(u => !u.isApproved)" :key="user" class="grid_item item">
					<h2 @click="toggleUserStatus(user)"> {{ user.user_id }} </h2>
				</div>
			</List>
		</span>
	</div>
</template>

<script>
import List from "@/layout/ListLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"

import { fetchGardenMeta } from "@/apiService.js"
import { mapState } from "vuex";

export default {
	components: {
		List,
		LC,
	},
	data() {
		return {
			userList: null,
			isUserListLoading: true,
		}
	},
	methods: {
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
		async toggleUserStatus(user) {
			this.isUserListLoading = true;
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/changestatus/${user.garden_id}/${user.user_id}`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});
			await this.fetchuser()
		},
	},
	async mounted() {
		this.selectedGarden = localStorage.getItem('selectedGarden')
		this.$store.commit("setGardenList", await fetchGardenMeta())
		this.fetchuser()
	}
}
</script>

<style lang="scss">
.userOverview {
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
</style>