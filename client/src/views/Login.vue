<template>
	<div class="hello">
		<h1>You will be redirected</h1>
	</div>
</template>
<script>
import { mapState } from "vuex";

export default {
	name: 'HelloWorld',
	props: {
		msg: String
	},
	computed: {
		...mapState({
			keycloak: (state) => state.keycloak,
		}),
	},
	async mounted() {
		console.log("Login", this.keycloak)
		if (this.keycloak?.authenticated) {
			await fetch(`${process.env.VUE_APP_PI_HOST}user/register`, {
				method: "POST",
				headers: {
					Authorization: `Bearer ${this.keycloak.token}`,
				},
			});
			if (localStorage.getItem("selectedGarden"))
				this.$router.push("overview")
			else
				this.$router.push("garden")
		}
		else {
			this.$router.push("/")
		}
	},
}
</script>
  <!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss"></style>