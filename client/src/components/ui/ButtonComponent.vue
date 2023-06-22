<template>
	<button :class="['button', { button_disabled: isLoading }]" @click="emitClick">
		<LoadingComponent v-if="isLoading" />
		<slot v-else></slot>
	</button>
</template>

<script>
import LoadingComponent from "@/components/ui/LoadingComponent.vue";

export default {
	components: {
		LoadingComponent
	},
	props: {
		isLoading: { type: Boolean, default: false }
	},
	methods: {
		emitClick() {
			if (!this.isLoading)
				this.$emit("clicked")
		}
	}
}
</script>

<style lang="scss">
.button {
	cursor: pointer;
	border-radius: 5px;
	height: 2rem;
	border: none;
	box-shadow: 0 0 2px 2px #69696921;
	background-color: #72db76;
	font-weight: 700;

	&_disabled {
		cursor: not-allowed
	}
}
</style>