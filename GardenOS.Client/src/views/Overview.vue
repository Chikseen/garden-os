<template>
    <LC v-if="isLoading" />
    <span v-else>
        <div v-if="gardenMeta" class="overview_title grid_item_titel">
            <span>
                <h1> Garden: </h1>
                <h1> {{ gardenMeta.garden_name }} </h1>
            </span>
            <span @click="ctc(gardenMeta.garden_id)">
                <h3> GardenID: </h3>
                <h3> {{ gardenMeta.garden_id }}
                    <CTC />
                </h3>
            </span>
            <span>
                <h3> User: </h3>
                <h3> {{ user?.given_name }} {{ user?.family_name }} : {{ userRoleName }} </h3>
            </span>
        </div>
        <DynamicGrid v-if="deviceData">
            <WeatherBox class="grid_item grid_item_weather" />
            <Informations class="grid_item" />
            <DeviceList :devices=deviceData />
            <div class="grid_item grid_item_settings grid_item_text">
                <h3>Garden settings</h3>
                <button @click="$router.push('/user')">User controll</button>
                <button v-if="user.gardenData.userRole >= 10" @click="$router.push('/devicesetting')">Device
                    setting</button>
            </div>
            <div class="grid_item grid_item_settings grid_item_text">
                <h3>You</h3>
                <button @click="$router.push('/garden')">Change Garden</button>
                <button @click="logout">Logout</button>
            </div>
        </DynamicGrid>
    </span>
</template>

<script>
import DynamicGrid from "@/layout/DynamicGridLayout.vue";
import DeviceList from "@/components/Devices/DeviceList.vue"
import WeatherBox from "@/components/WeatherBox.vue"
import LC from "@/components/ui/LoadingComponent.vue"
import Informations from "@/components/InformationsComponent.vue"
import CTC from "@/assets/CopyToClipboardIcon.vue"
import HubControll from "@/components/HubControll.vue"

import { fetchGardenMeta, fetchUser } from "@/services/apiService.js"
import { getRoleNameById } from "@/services/userroleService.js"
import { mapState, mapActions } from "vuex";

export default {
    components: {
        DynamicGrid,
        DeviceList,
        WeatherBox,
        HubControll,
        Informations,
        LC,
        CTC,
    },
    data() {
        return {
            isLoading: true,
        }
    },
    methods: {
        ctc(value) {
            navigator.clipboard.writeText(value);
        },
        ...mapActions({
            fetchDevices: 'fetchDevices',
            logout: 'logout'
        })
    },
    computed: {
        userRoleName() {
            return getRoleNameById(this.user.gardenData.userRole)
        },
        ...mapState({
            gardenMeta: (state) => state.gardenMeta,
            deviceData: (state) => state.deviceData,
            keycloak: (state) => state.keycloak,
            user: (state) => state.user,
        }),
    },
    async mounted() {
        const gardenid = localStorage.getItem("selectedGarden")
        if (gardenid?.length > 0) {
            this.$store.commit("setUser", await fetchUser(gardenid))
            this.$store.commit("setGardenList", await fetchGardenMeta())
            this.fetchDevices()
            this.$hub.invoke("setUserToGarden", gardenid);
            this.isLoading = false
        }
        else {
            this.$router.push("garden")
        }
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
            h3 {
                width: 100%;
                position: relative;

                svg {
                    position: absolute;
                    top: 0;
                    left: -1.5rem;
                    height: 1.5rem;
                    width: 1rem;
                }
            }
        }
    }
}

.tomorrow {
    max-width: 750px;
}
</style>
services/apiService.js