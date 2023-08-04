<template>
	<div class="temp-icon_wrapper">
		<div class="temp-icon">
			<svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 55 55" width="100%" height="100%">
				<path :fill="color" d="M27.5 0A1 1 0 0127.5 55 1 1 0 0127.5 0M27.5 1A1 1 0 0027.5 54 1 1 0 0027.5 1" />
				<path :fill="color"
					d="M25 38C22.464 39.093 20.763 41.592 20.743 43.994 20.821 45.478 21.427 46.884 22.482 47.88 23.692 48.935 24.942 49.677 26.427 49.833 28.126 49.892 29.473 49.091 30.606 47.919 31.544 46.787 32.032 45.732 32.247 43.974 32.051 41.416 30.88 39.697 28.103 37.941V5.41A1 1 0 0025.018 5.49V37.582" />
			</svg>
			<div class="temp-icon_cover" :style="`bottom: ${((value + 10) * 2)}%;`"></div>
		</div>
		<svg class="temp-icon" version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 55 55" width="100%"
			height="100%">
			<path :fill="color"
				d="M31 37V5.88C31 3.55 29.11 1.66 26.78 1.66 24.45 1.66 22.56 3.55 22.56 5.88V36.99C19.92 38.5 18.3 41.27 18.28 44.33 18.27 46.53 19.13 48.63 20.69 50.24 22.25 51.86 24.32 52.79 26.52 52.85 26.61 52.85 26.7 52.85 26.78 52.85 29 52.85 31.09 52 32.69 50.45 34.35 48.84 35.27 46.67 35.27 44.35 35.28 41.29 33.65 38.51 31 37ZM31.58 49.31C30.23 50.62 28.45 51.31 26.57 51.25 24.8 51.2 23.12 50.44 21.85 49.13 20.58 47.81 19.88 46.11 19.89 44.34 19.9 41.72 21.37 39.37 23.72 38.2 23.99 38.06 24.16 37.79 24.16 37.48V5.88C24.16 4.44 25.33 3.26 26.78 3.26 28.22 3.26 29.4 4.43 29.4 5.88V37.47C29.4 37.77 29.57 38.05 29.84 38.19 32.2 39.37 33.67 41.73 33.67 44.36 33.67 46.24 32.93 48 31.58 49.31Z" />
		</svg>
	</div>
</template>

<script>
export default {
	props: {
		value: { type: Number }
	},
	computed: {
		color() {
			if (this.value < -5)
				return "#18a1d2"
			if (this.value < 0)
				return "#56a6a7"
			if (this.value < 5)
				return "#90aa7d"
			if (this.value < 10)
				return "#b8ad61"
			if (this.value < 15)
				return "#deb046"
			if (this.value < 20)
				return "#f8ab31"
			if (this.value < 25)
				return "#f5962c"
			if (this.value < 30)
				return "#f18127"
			if (this.value < 35)
				return "#ee6d22"
			return "#eb581d"
		},
	}
}
</script>

<style lang="scss">
.temp-icon {
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
		transition: bottom 1s;
	}

	&_spinner {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		animation: tempSpinner 20s infinite linear;
	}
}

@keyframes tempSpinner {
	from {
		transform: rotateZ(0deg);
	}

	to {
		transform: rotateZ(360deg);
	}
}

@media only screen and (max-width: 450px) {
	.temp-icon {
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