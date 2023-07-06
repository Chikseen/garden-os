<template>
    <RenderComponent v-if="is3dView" />
    <div v-else>
        <div v-if="gardenMeta" class="overview_title grid_item_titel">
            <h1>Garden: {{ gardenMeta.garden_name }}</h1>
            <h3>User: {{ gardenMeta.user_name }}</h3>
        </div>
        <DynamicGrid>
            <WeatherBox />
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
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import DeviceBox from "@/components/Devices/DeviceBox.vue"
import ForLoopWrapper from "@/components/ForLoopWrapper.vue"
import WeatherBox from "@/components/WeatherBox.vue"

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"

import { mapState } from "vuex";

import Keycloak from "keycloak-js"

export default {
    components: {
        DynamicGrid,
        DeviceBox,
        ForLoopWrapper,
        WeatherBox
    },
    methods: {
        async logout() {
            localStorage.clear();
            const keycloak = new Keycloak({
                url: "https://auth.drunc.net",
                realm: process.env.VUE_APP_AUTH_REALM,
                clientId: process.env.VUE_APP_AUTH_CLIENT_ID,

            });
            await keycloak
                .init({})
            keycloak.logout({ redirectUri: process.env.VUE_APP_AUTH_LOGOUT })
        }
    },
    computed: {
        ...mapState({
            gardenMeta: (state) => state.gardenMeta,
            is3dView: (state) => state.is3dView,
            deviceData: (state) => state.deviceData,
            keycloak: (state) => state.keycloak,
        }),
    },
    async mounted() {
        this.$store.commit("setGardenList", await fetchGardenMeta())
        this.$store.commit("setDeviceData", await fetchDevices("accd30d2-7392-40b7-8a08-6d9ac9cc22b6"))
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
