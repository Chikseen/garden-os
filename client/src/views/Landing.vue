<template>
    <div class="landing_wrapper">
        <img src="@/assets/gardenOSTransparent.png" alt="title image Garden os">
        <span v-if="!registerMode" class="landing_login_wrapper">
            <h4 v-if="showErrorMessage" class="failedText">Invalid login</h4>
            <form class="landing_login" onsubmit="return false">
                <input type="text" @change="(e) => AuthId = e.target.value" placeholder="User id" :value="AuthId"
                    autocomplete="username" />
                <input type="password" @change="(e) => AuthApiKey = e.target.value" placeholder="API key"
                    :value="AuthApiKey" autocomplete="current-password" />
                <ButtonComponent @clicked="checkUser" :isLoading="isLoading" type="submit"> Login </ButtonComponent>
            </form>
            <p @click="registerMode = !registerMode">Or register a new account</p>
        </span>
        <span v-else class="landing_login_wrapper">
            <h4 v-if="showErrorMessage" class="failedText">Register attempt failed</h4>
            <form class="landing_login" onsubmit="return false">
                <input type="text" @change="(e) => userName = e.target.value" placeholder="User name" />
                <input type="text" @change="(e) => gardenId = e.target.value" placeholder="Garden id"
                    autocomplete="gardenID" />
                <ButtonComponent @clicked="registerAccount" :isLoading="isLoading">Register</ButtonComponent>
            </form>
            <p @click="registerMode = !registerMode">You have allready have a account?</p>
        </span>
        <p>{{ t }}</p>
        <p>{{ t1 }}</p>
        <p>{{ t3 }}</p>
    </div>
</template>

<script>
import ButtonComponent from "@/components/ui/ButtonComponent.vue"

export default {
    components: {
        ButtonComponent
    },
    data: () => {
        return {
            values: null,
            AuthId: "",
            AuthApiKey: "",
            registerMode: false,
            gardenId: "",
            userName: "",
            showErrorMessage: false,
            isLoading: false,
            t: "",
            t1: "",
            t3: "",
        };
    },
    methods: {
        async checkUser() {
            this.isLoading = true;
            if (this.AuthId?.length > 0) {
                const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/${this.AuthId}/validate`, {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${this.AuthApiKey}`,
                    },
                });
                const res = await json.json();
                if (res.status > 200)
                    this.showErrorMessage = true
                if (res == true) {
                    localStorage.setItem("id", this.AuthId.toString());
                    localStorage.setItem("apiToken", this.AuthApiKey.toString());
                    this.$store.commit("setAuthState", true);
                } else {
                    this.$store.commit("setAuthState", false);
                    this.showErrorMessage = true
                }
            }
            this.isLoading = false;
        },
        async registerAccount() {
            this.showErrorMessage = false
            this.isLoading = true;
            const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/register`, {
                method: "POST",
                body: JSON.stringify({ garden_id: this.gardenId, user_name: this.userName }),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            });
            this.t3 = {
                method: "POST",
                body: JSON.stringify({ garden_id: this.gardenId, user_name: this.userName }),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            }

            const res = await json.json();

            /*        const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/datalog`, {
                      method: "POST",
                      body: JSON.stringify(this.timeframe),
                      headers: {
                        'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                      },
                    });
                    const chartData = await initData.json()*/

            this.t = res
            if (res.status > 200) {
                this.showErrorMessage = true
                this.t1 = res
            }
            else {
                this.registerMode = false
                this.AuthId = res.user_id
                this.AuthApiKey = res.api_key
            }
            this.isLoading = false;
        }
    },
    async mounted() {
        this.AuthId = localStorage.getItem("id");
        this.AuthApiKey = localStorage.getItem("apiToken");
        await this.checkUser();
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