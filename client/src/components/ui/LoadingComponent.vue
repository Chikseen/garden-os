<template>
	<GardenOSIcon class="LoadingIcon" />
</template>

<script>
import GardenOSIcon from "@/assets/GardenOSIcon.vue"

export default {
	components: {
		GardenOSIcon
	}
}
</script>

<style lang="scss">
.LoadingIcon {
	width: 100%;
	height: 100%;

	img {
		max-width: 100%;
		max-height: 100%;
		animation: IconSpinner 4s infinite;
	}
}

@keyframes IconSpinner {
	0% {
		transform: rotateZ(0);
	}

	10% {
		transform: rotateZ(15deg);
	}

	20% {
		transform: rotateZ(75deg);
	}

	30% {
		transform: rotateZ(105deg);
	}

	45% {
		transform: rotateZ(165deg);
	}

	55% {
		transform: rotateZ(195deg);
	}

	70% {
		transform: rotateZ(255deg);
	}

	80% {
		transform: rotateZ(285deg);
	}

	90% {
		transform: rotateZ(345deg);
	}

	100% {
		transform: rotateZ(360deg);
	}
}
</style>