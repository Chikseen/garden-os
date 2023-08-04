<template>
	<div class="list">
		<slot></slot>
	</div>
</template>

<style lang="scss">
.list {
	display: flex;
	flex-direction: column;
	gap: 15px;
	margin: 15px;

	&_item {
		border-radius: 24px;
		box-shadow: 0 0 2px 2px #69696921;
		overflow: hidden;
	}
}
</style>