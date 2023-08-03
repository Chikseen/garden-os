<template>
	<div class="uvi-icon_wrapper">
		<div class="uvi-icon">
			<svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 30 30" width="100%" height="100%">
				<path :fill="style.color" d="M15 1A1 1 0 0115 29 1 1 0 0115 1M15 2A1 1 0 0015 28 1 1 0 0015 2" />
			</svg>
			<div class="uvi-icon_cover" :style="`bottom: ${value * 10}%;`"></div>
		</div>
		<svg class="uvi-icon" version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 30 30" width="100%"
			height="100%">
			<path :fill="style.color" d="M15 9A1 1 0 0015 21 1 1 0 0015 9" />
		</svg>
		<div class="uvi-icon_spinner">
			<svg class="uvi-icon" v-for="i in style.sunBeam" :key="i"
				:style="`transform: rotateZ(${360 / style.sunBeam * i}deg);`" version="1.1"
				xmlns="http://www.w3.org/2000/svg" viewBox="0 0 30 30" width="100%" height="100%">
				<path :fill="style.color" d="M14.5 4A1.5 1 0 0115.5 4L15.5 7A1.5 1 0 0114.5 7L14.5 4" />
			</svg>
		</div>
	</div>
</template>

<script>
export default {
	props: {
		value: { type: Number }
	},
	computed: {
		style() {
			if (this.value < 2)
				return {
					sunBeam: 0,
					color: "#a9a9a9"
				}
			if (this.value < 4)
				return {
					sunBeam: 8,
					color: "#4f42b5"
				}
			if (this.value < 6)
				return {
					sunBeam: 10,
					color: "#7ec850"
				}
			if (this.value < 8)
				return {
					sunBeam: 12,
					color: "#ff6700"
				}
			return {
				sunBeam: 14,
				color: "#8b0000"
			}
		},
	}
}
</script>

<style lang="scss">
.uvi-icon {
	position: absolute;
	top: 0;
	left: 0;
	width: 100%;
	height: 100%;
	font-size: 2px;

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
	}

	&_cover {
		position: absolute;
		left: 0;
		width: 100%;
		height: 100%;
		background-color: rgb(255, 255, 255);
	}

	&_spinner {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		animation: uviSpinner 20s infinite linear;
	}
}

@keyframes uviSpinner {
	from {
		transform: rotateZ(0deg);
	}

	to {
		transform: rotateZ(360deg);
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