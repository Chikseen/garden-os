<template>
    <div class="landing_wrapper">
        <img src="@/assets/gardenOSTransparent.png" alt="title image Garden os">
        <div class="landing_login">
            <div class="landing_login_box">
                <h3>User ID</h3>
                <input type="text" @change="insertID" />
            </div>
            <div class="landing_login_box">
                <h3>Api Key</h3>
                <input type="text" @change="insertApiKey" />
            </div>
            <button @click="checkUser">Login</button>
        </div>
        <h6>QUICK DUMMY LOGIN</h6>
        <h6 @click="ctc('1a667139-d648-4745-8529-a296c6de6f05')">
            1a667139-d648-4745-8529-a296c6de6f05</h6>
        <h6
            @click="ctc('OExfKUsFUh8bpVaR8soHNGhvFcwMXAcsLLQazmzdDumn0nSKMne2lsMJCgkPoEF2rZuUkWRMlQ7lK4WH3TNnTe16adkHeVCVwqhmZXASrcBaZzQ5j2qVQoubRDMiVbOW')">
            OExfKUsFUh8bpVaR8soHNGhvFcwMXAcsLLQazmzdDumn0nSKMne2lsMJCgkPoEF2rZuUkWRMlQ7lK4WH3TNnTe16adkHeVCVwqhmZXASrcBaZzQ5j2qVQoubRDMiVbOW
        </h6>
    </div>
</template>

<script>
export default {
    data: () => {
        return {
            values: null,
            AuthId: "",
            AuthApiKey: "",
        };
    },
    methods: {
        insertID(e) {
            this.AuthId = e.target.value;
        },
        insertApiKey(e) {
            this.AuthApiKey = e.target.value;
        },
        async checkUser() {
            const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/${this.AuthId}/validate`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${this.AuthApiKey}`,
                },
            });
            const res = await json.json();
            console.log(res);
            if (res == true) {
                localStorage.setItem("id", this.AuthId.toString());
                localStorage.setItem("apiToken", this.AuthApiKey.toString());
                this.$store.commit("setAuthState", true);
            } else this.$store.commit("setAuthState", false);
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
        width: 100%;
        max-width: 750px;
        margin: auto;
        overflow: auto;
        height: max-content;
        user-select: none;

        img {
            max-width: 100%;
            height: 100vh;
            max-height: 750px;
            object-fit: contain;
        }

        h6 {
            word-break: break-all;
            user-select: text;
        }
    }

    &_login {
        display: flex;
        flex-direction: column;
        margin: 0 auto;

        &_box {
            margin: auto;
        }

        h3 {
            margin: 0.5rem;
        }

        button {
            width: 100px;
            margin: 5px auto;
        }
    }
}
</style>