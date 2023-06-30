<template>
	<div class="hello">
		<h1>You will be redirectedf</h1>
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
	methods: {
		login() {
			console.log("Login", this.keycloak)
			if (this.keycloak?.authenticated)
				this.$router.push("overview")
			else
				this.$router.push("")
		}
	},
	mounted() {
		this.login()
	},
}
</script>
  <!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss"></style>