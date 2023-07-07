<template>
    <div v-if="gardenMeta" class="overview_title grid_item_titel">
        <h1>Garden: {{ gardenMeta.garden_name }} <h5>{{ gardenMeta.garden_id }}</h5>
        </h1>
        <h3>User: {{ gardenMeta.user_name }}</h3>
    </div>
    <DynamicGrid v-if="deviceData">
        <WeatherBox class="grid_item grid_item_large" />
        <DeviceList :devices=deviceData?.devices></DeviceList>
        <div class="grid_item grid_item_settings">
            <button @click="logout">Logout</button>
            <button @click="$router.push('/garden')">Change Garden</button>
        </div>
    </DynamicGrid>
</template>

<script>
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import DeviceList from "@/components/Devices/DeviceList.vue"
import WeatherBox from "@/components/WeatherBox.vue"

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"
import { mapState } from "vuex";

import Keycloak from "keycloak-js"

export default {
    components: {
        DynamicGrid,
        DeviceList,
        WeatherBox
    },
    methods: {
        async logout() {
            localStorage.clear();
            console.log("RDURL", process.env.VUE_APP_AUTH_LOGOUT)
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
            deviceData: (state) => state.deviceData,
            keycloak: (state) => state.keycloak,
        }),
    },
    async mounted() {
        this.$store.commit("setGardenList", await fetchGardenMeta())
        this.$store.commit("setDeviceData", await fetchDevices(localStorage.getItem("selectedGarden")))
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
