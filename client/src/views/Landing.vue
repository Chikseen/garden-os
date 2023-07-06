<template>
    <div class="landing_wrapper">
        <img src="@/assets/gardenOSTransparent.png" alt="title image Garden os">
        <button @click="login">Login</button>
    </div>
</template>

<script>
import ButtonComponent from "@/components/ui/ButtonComponent.vue"
import Login from "@/views/Login.vue"
import Keycloak from "keycloak-js"

export default {
    components: {
        ButtonComponent,
        Login
    },
    data: () => {
        return {};
    },
    methods: {
        login() {
            if (localStorage.getItem("accessToken")?.length > 1)
                this.$router.push("/overview")
            else {
                const keycloak = new Keycloak({
                    url: "https://auth.drunc.net",
                    realm: process.env.VUE_APP_AUTH_REALM,
                    clientId: process.env.VUE_APP_AUTH_CLIENT_ID,
                });
                keycloak
                    .init({
                        onLoad: "check-sso",
                        redirectUri: process.env.VUE_APP_AUTH_REDIRECT
                    })
            }
        },
    },
    async mounted() {
        this.login()
    },
}
</script>

<style lang="scss">
.landing {
    &_wrapper {
        display: flex;
        flex-direction: column;
        justify-content: space-evenly;
        gap: 1rem;
        width: calc(100% - 20px);
        max-width: 750px;
        height: calc(100vh - 20px);
        max-height: 750px;
        margin: auto auto;
        padding: 10px;
        user-select: none;

        img {
            max-width: 100%;
            object-fit: contain;
        }
    }

    &_login,
    &_login_wrapper {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        gap: 1rem;
        margin: 0 auto;
        width: 75%;
        max-width: 500px;

        p {
            margin: 0 auto;
            color: black;
        }

        input {
            border-radius: 5px;
            height: 2rem;
            border: none;
            box-shadow: 0 0 2px 2px #69696921;
        }
    }
}

.failedText {
    margin: 0 auto;
    color: black;
    background-color: rgb(236, 96, 91);
    border-radius: 5px;
    padding: 5px;
}
</style>