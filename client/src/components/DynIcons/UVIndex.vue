<template>
	<div class="uvi-icon_wrapper">
		<div v-for="(item, index) in size" :key="index" class="uvi-icon" :style="`background-color: ${color};`">{{ item }}
		</div>
	</div>
</template>

<script>
export default {
	props: {
		value: { type: Number }
	},
	computed: {
		size() {
			return ".".repeat((this.value / 10).toFixed(0) * 1)
		},
		color() {
			if (this.size.length < 3)
				return "#312F36"
			if (this.size.length < 5)
				return "#00A1B9"
			if (this.size.length < 7)
				return "#66A586"
			if (this.size.length < 9)
				return "#E86100"
			return "#E31937"
		}
	}
}
</script>

<style lang="scss">
.uvi-icon {
	font-size: 2px;
	width: 100%;

	&_wrapper {
		position: relative;
		display: flex;
		flex-direction: column;
		justify-content: flex-end;
		min-height: 50px;
		min-width: 50px;
		max-height: 50px;
		max-width: 50px;
		border-radius: 10px;
		gap: 2px;
		overflow: hidden;
		box-shadow: 0 0 5px 2px #c9c9c986;

		span {
			max-height: 10%;
		}
	}
}

@media only screen and (max-width: 450px) {
	.uvi-icon {
		font-size: 1.6px;

		&_wrapper {
			min-height: 35px;
			min-width: 35px;
			max-height: 35px;
			max-width: 35px;
		}
	}
}
</style>