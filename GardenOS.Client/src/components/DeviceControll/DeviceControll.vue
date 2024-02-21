<template>
	<div v-for="(controller, index) in controlls?.controllers" :key="index" class="grid_item">
		<div class="controller_wrapper">
			<span class="controller_wrapper_titel">
				<h2> {{ controller.name }} </h2>
			</span>
			<ClickAndHoldButton @trigger="() => console.log('hi')">
				Do Something
			</ClickAndHoldButton>
			<button>
				Automation
			</button>
		</div>
	</div>
</template>

<script>
import { mapState } from "vuex";
import ClickAndHoldButton from "@/components/ClickAndHoldButton.vue"

export default {
	components: {
		ClickAndHoldButton
	},
	mounted() {

	},
	computed: {
		...mapState({
			controlls: (state) => state.controlls,
		}),
	},
}
</script>

<style lang="scss">
.controller {
	&_wrapper {
		cursor: pointer;
		display: flex;
		flex-direction: column;
		justify-content: center;
		gap: 15px;
		padding: 10px 5px;
		background-color: #ffffff;
		height: calc(100% - 20px);

		&_icon {
			margin: auto 0;
		}

		&_titel {
			display: flex;
			flex-direction: column;
			justify-content: space-evenly;

			h1,
			h2,
			h3,
			h5,
			p {
				text-align: center;
			}
		}

	}
}
</style>