<template>
	<div class="grid">
		<slot></slot>
	</div>
</template>

<style lang="scss">
.grid {
	display: grid;
	grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
	grid-auto-rows: 1fr;
	gap: 15px;
	max-width: 1500px;
	margin: 0 auto;
	padding: 15px;

	&_item {
		border-radius: 24px;
		box-shadow: 0 0 2px 2px #69696921;
		overflow: hidden;

		&_titel {
			grid-column: 1 / -1;
		}

		&_large {
			grid-row-start: span 2;
		}

		&_xlarge {
			grid-row-start: span 3;
		}

		&_settings {
			padding: 15px;
			display: flex;
			flex-direction: column;
			gap: 15px;

			button {
				cursor: pointer;
				width: 100%;
			}
		}
	}
}

.upcoming-widget-square__root--KGAv {
	border-radius: 10px !important;
	border: 10px solid #5f5e5e21 !important;
}

@media only screen and (max-width: 350px) {
	.grid {
		grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
	}
}
</style>