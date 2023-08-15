<template>
	<button @mousedown.stop="mouseDown" @touchstart.stop="mouseDown" @mouseup.stop="mouseUp" @touchend.stop="mouseUp" ref="cahbutton">
		<slot></slot>
	</button>
</template>

<script>
export default {
	data() {
		return {
			timer: null
		}
	},
	methods: {
		mouseDown() {
			this.timer = setTimeout(() => {
				this.$emit("trigger")
			}, 3000);
			this.$refs.cahbutton.classList.add("clickandholdanimation")
		},
		mouseUp() {
			clearTimeout(this.timer);
			this.$refs.cahbutton.classList.remove("clickandholdanimation")
		},
	},
}
</script>

<style lang="scss">
.clickandholdanimation {
	position: relative;
	overflow: hidden;
	user-select: none;

	&::after {
		content: "";
		position: absolute;
		top: 0;
		left: -100%;
		width: 100%;
		height: 100%;
		background-color: #e26756;
		border-radius: 10px;
		animation: cah 3s linear 0s;
	}
}

@keyframes cah {
	from {
		left: -100%;
	}

	to {
		left: 0;
	}
}
</style>