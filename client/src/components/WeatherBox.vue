<template>
	<div v-if="gardenMeta?.weather_location_id" class="tomorrow grid_item grid_item_weather" data-language="DE"
		data-unit-system="METRIC" data-skin="light" data-widget-type="upcoming"
		style="padding-bottom: 22px; position:relative;" :data-location-id="gardenMeta.weather_location_id">
		<a href="https://www.tomorrow.io/weather-api/" rel="nofollow noopener noreferrer" target="_blank"
			style="position: absolute; bottom: 0; transform: translateX(-50%); left: 50%;">
			<img alt="Powered by the Tomorrow.io Weather API"
				src="https://weather-website-client.tomorrow.io/img/powered-by.svg" width="250" height="18" />
		</a>
	</div>
</template>

<script>
import { fetchGardenMeta } from "@/apiService.js"
import { mapState } from "vuex";

export default {
	computed: {
		...mapState({
			gardenMeta: (state) => state.gardenMeta,

		}),
	},
	methods: {
		load(d, s, id) {
			if (d.getElementById(id)) {
				if (window.__TOMORROW__) {
					window.__TOMORROW__.renderWidget();
				}
				return;
			}
			const fjs = d.getElementsByTagName(s)[0];
			const js = d.createElement(s);
			js.id = id;
			js.src = "https://www.tomorrow.io/v1/widget/sdk/sdk.bundle.min.js";
			fjs.parentNode.insertBefore(js, fjs);
		}
	},
	async mounted() {
		this.load(document, "script", "tomorrow-sdk")
		this.$store.commit("setGardenMeta", await fetchGardenMeta())
	}
}
</script>