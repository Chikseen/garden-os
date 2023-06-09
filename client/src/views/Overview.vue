<template>
    <RenderComponent v-if="is3dView" />
    <div v-else>
        <div v-if="gardenMeta" class="overview_title grid_item_titel">
            <h1>Garden: {{ gardenMeta.garden_name }}</h1>
            <h3>User: {{ gardenMeta.user_name }}</h3>
        </div>
        <DynamicGrid>
            <div v-if="gardenMeta?.weather_location_id" class="tomorrow grid_item grid_item_weather" data-language="DE"
                data-unit-system="METRIC" data-skin="light" data-widget-type="upcoming"
                style="padding-bottom: 22px; position:relative;" :data-location-id="gardenMeta.weather_location_id">
                <a href="https://www.tomorrow.io/weather-api/" rel="nofollow noopener noreferrer" target="_blank"
                    style="position: absolute; bottom: 0; transform: translateX(-50%); left: 50%;">
                    <img alt="Powered by the Tomorrow.io Weather API"
                        src="https://weather-website-client.tomorrow.io/img/powered-by.svg" width="250" height="18" />
                </a>
            </div>
            <ForLoopWrapper v-if="deviceData?.devices">
                <DeviceBox v-for="device in deviceData?.devices" :key="device.device_id" :device="device" />
            </ForLoopWrapper>
            <div>
                <h3>Here is just debug info</h3>
                <p @click="$store.commit('set3dView', true)">Click here for 3d view</p>
                <h6 @click="logout">Logout</h6>
            </div>
        </DynamicGrid>
    </div>
</template>

<script>
import RenderComponent from "@/components/Render/RenderComponent.vue";
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import DeviceBox from "@/components/Devices/DeviceBox.vue"
import ForLoopWrapper from "@/components/ForLoopWrapper.vue"

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"

import { mapState } from "vuex";

(function (d, s, id) {
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
})(document, 'script', 'tomorrow-sdk');

export default {
    components: {
        RenderComponent,
        DynamicGrid,
        DeviceBox,
        ForLoopWrapper
    },
    methods: {
        logout() {
            this.$store.commit('setAuthState', false)
            localStorage.clear();
        }
    },
    computed: {
        ...mapState({
            gardenMeta: (state) => state.gardenMeta,
            is3dView: (state) => state.is3dView,
            deviceData: (state) => state.deviceData,
        }),
    },
    async mounted() {
        this.$store.commit("setGardenMeta", await fetchGardenMeta())
        this.$store.commit("setDeviceData", await fetchDevices())
    }
}
</script>
<style lang="scss">
.overview {
    &_title {
        max-width: 750px;
        width: 100%;
        margin: 0 auto;
        padding: 10px;
    }
}

.tomorrow {
    max-width: 750px;
}
</style>
