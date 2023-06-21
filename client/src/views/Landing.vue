<template>
    <div class="landing_wrapper">
        <img src="@/assets/gardenOSTransparent.png" alt="title image Garden os">
        <span v-if="!registerMode" class="landing_login_wrapper">
            <form class="landing_login" onsubmit="return checkUser();">
                <input type="text" @change="insertID" placeholder="User id" :value="AuthId" />
                <input type="password" @change="insertApiKey" placeholder="API key" :value="AuthApiKey" />
            </form>
            <button @click="checkUser">Login</button>
            <p @click="registerMode = !registerMode">Or register a new account</p>
        </span>
        <span v-else class="landing_login_wrapper">
            <h3 v-if="showErrorMessage" class="failedText">Register attempt failed</h3>
            <div class="landing_login">
                <input type="text" @change="insertUserName" placeholder="User name" />
                <input type="text" @change="insertGardenId" placeholder="Garden id" />
            </div>
            <button @click="registerAccount">Register</button>
            <p @click="registerMode = !registerMode">You have allready have a account?</p>
        </span>
    </div>
</template>

<script>
export default {
    data: () => {
        return {
            values: null,
            AuthId: "",
            AuthApiKey: "",
            registerMode: false,
            gardenId: "",
            userName: "",
            showErrorMessage: false
        };
    },
    methods: {
        insertID(e) {
            this.AuthId = e.target.value;
        },
        insertApiKey(e) {
            this.AuthApiKey = e.target.value;
        },
        insertGardenId(e) {
            this.gardenId = e.target.value;
        },
        insertUserName(e) {
            this.userName = e.target.value;
        },
        async checkUser() {
            const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/${this.AuthId}/validate`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${this.AuthApiKey}`,
                },
            });
            const res = await json.json();
            if (res == true) {
                localStorage.setItem("id", this.AuthId.toString());
                localStorage.setItem("apiToken", this.AuthApiKey.toString());
                this.$store.commit("setAuthState", true);
            } else this.$store.commit("setAuthState", false);
        },
        async registerAccount() {
            this.showErrorMessage = false
            const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/register`, {
                method: "POST",
                body: JSON.stringify({ garden_id: this.gardenId, user_name: this.userName }),
                headers: {
                    "Content-Type": "application/json",
                },
            });
            const res = await json.json();
            if (res.status > 200)
                this.showErrorMessage = true
            else {
                this.registerMode = false
                this.AuthId = res.user_id
                this.AuthApiKey = res.api_key
            }

            console.log(res)
        },
        async ctc(text) {
            try {
                await navigator.clipboard.writeText(text);
            } catch (e) {
            }
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

        button,
        input {
            border-radius: 5px;
            height: 2rem;
            border: none;
            box-shadow: 0 0 2px 2px #69696921;
        }

        button {
            background-color: #72db76;
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