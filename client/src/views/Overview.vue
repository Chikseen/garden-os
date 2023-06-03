<template>
    <RenderComponent v-if="is3dView" />
    <div v-else>
        <div v-if="gardenMeta" class="overview_title">
            <h1>Garden: {{ gardenMeta.garden_name }}</h1>
            <h3>User: {{ gardenMeta.user_name }}</h3>
        </div>
        <DeviceList :devices="deviceData" />
        <p @click="$store.commit('set3dView', true)">Click here for 3d view</p>
        <h6 @click="logout">Logout</h6>
    </div>
</template>

<script>
import RenderComponent from "@/components/Render/RenderComponent.vue";
import DeviceList from "@/components/Devices/DeviceList.vue"

import { mapState } from "vuex";

export default {
    components: {
        RenderComponent,
        DeviceList
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
        const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/garden`, {
            method: "GET",
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
            },
        });
        const res = await initData.json()
        console.log(res);
        this.$store.commit("setGardenMeta", res)
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
</style>
