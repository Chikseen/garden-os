<template>
	<div class="userOverview_wrapper">
		<h1>User Settings</h1>
		<LC v-if="isUserListLoading" />
		<span v-else>
			<List>
				<div v-for="user in userList?.userList.filter(u => u.user_id != user.preferred_username)" :key="user"
					class="grid_item item userOverview_item">
					<div>
						<h4> Name: {{ user.given_name }} {{ user.family_name }} </h4>
						<h4> ID: {{ user.user_id }} </h4>
						<h4> Role: {{ nameRole(user.userrole_id) }}</h4>
					</div>
					<div class="userOverview_item_buttons">
						<button :class="{ userOverview_item_buttons_button_active: user.userrole_id == '20' }"
							@click="toggleUserStatus(user.user_id, '20')">Admin</button>
						<button :class="{ userOverview_item_buttons_button_active: user.userrole_id == '10' }"
							@click="toggleUserStatus(user.user_id, '10')">Maintainer</button>
						<button :class="{ userOverview_item_buttons_button_active: user.userrole_id == '0' }"
							@click="toggleUserStatus(user.user_id, '0')">Viewer</button>
					</div>
				</div>
			</List>
		</span>
	</div>
</template>

<script>
import List from "@/layout/ListLayout.vue";
import LC from "@/components/ui/LoadingComponent.vue"

import { mapState } from "vuex";
import { fetchGardenMeta, fetchUser } from "@/services/apiService.js"
import { getRoleNameById } from "@/services/userroleService.js"

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
		nameRole(id) {
			return getRoleNameById(id)
		},
		async fetchuser() {
			const response = await fetch(`${process.env.VUE_APP_PI_HOST}garden/${this.selectedGarden}/users`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
				},
			});

			this.userList = await response.json();

			this.isUserListLoading = false;
		},
		async toggleUserStatus(userId, roleId) {
			this.isUserListLoading = true;
			await fetch(`${process.env.VUE_APP_PI_HOST}user/changestatus`, {
				method: "POST",
				body: JSON.stringify({
					"user_id": userId,
					"userrole_id": roleId,
					"garden_id": localStorage.getItem("selectedGarden")
				}),
				headers: {
					Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
			});
			await this.fetchuser()
		},
	},
	computed: {
		...mapState({
			user: (state) => state.user,
		}),
	},
	async mounted() {
		this.$store.commit("setUser", await fetchUser(localStorage.getItem("selectedGarden")))
		this.selectedGarden = localStorage.getItem('selectedGarden')
		this.$store.commit("setGardenList", await fetchGardenMeta())
		this.fetchuser()
	}
}
</script>

<style lang="scss">
.userOverview {
	&_wrapper {
		max-width: 500px;
		width: 100%;
		margin: 0 auto;

		h4,
		p {
			padding-left: 15px;
		}
	}

	&_item {
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		gap: 25px;

		&_buttons {
			display: flex;
			flex-direction: column;
			gap: 5px;

			&_button {
				&_active {
					cursor: not-allowed;
					background-color: #00800080;
				}
			}
		}
	}
}
</style>services/apiService.js