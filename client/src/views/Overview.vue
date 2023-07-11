<template>
    <LC v-if="isLoading" />
    <span v-else>
        <div v-if="gardenMeta" class="overview_title grid_item_titel">
            <span>
                <h1> Garden: </h1>
                <h1> {{ gardenMeta.garden_name }} </h1>
            </span>
            <span>
                <h5> GardenID: </h5>
                <h5> {{ gardenMeta.garden_id }} </h5>
            </span>
            <span>
                <h3> User: </h3>
                <h3> {{ gardenMeta.user_name }} </h3>
            </span>
        </div>
        <DynamicGrid v-if="deviceData">
            <WeatherBox class="grid_item grid_item_xlarge" />
            <DeviceList :devices=deviceData?.devices></DeviceList>
            <div class="grid_item grid_item_settings">
                <button @click="logout">Logout</button>
                <button @click="$router.push('/garden')">Change Garden</button>
            </div>
        </DynamicGrid>
    </span>
</template>

<script>
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import DeviceList from "@/components/Devices/DeviceList.vue"
import WeatherBox from "@/components/WeatherBox.vue"
import LC from "@/components/ui/LoadingComponent.vue"

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"
import { mapState } from "vuex";

import Keycloak from "keycloak-js"

export default {
    components: {
        DynamicGrid,
        DeviceList,
        WeatherBox,
        LC,
    },
    data() {
        return {
            isLoading: true
        }
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
            deviceData: (state) => state.deviceData,
            keycloak: (state) => state.keycloak,
        }),
    },
    async mounted() {
        this.$store.commit("setGardenList", await fetchGardenMeta())
        this.$store.commit("setDeviceData", await fetchDevices(localStorage.getItem("selectedGarden")))
        this.isLoading = false
        console.log(this.$hub)
        this.$hub.invoke("setUserToGarden", localStorage.getItem("selectedGarden"));
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

        span {
            display: flex;
            justify-content: space-between;

            h1,
            h3,
            h5 {
                width: 100%;
            }

            h5 {
                font-size: 0.75rem;
            }
        }
    }
}

.tomorrow {
    max-width: 750px;
}
</style>
