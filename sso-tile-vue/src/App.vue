<template>
    <div id="app">
        <div id="nav">
            <h4>PCF SSO Tile + Vue.js</h4>
            <router-link to="/">Home</router-link>
            <router-link to="/secure">Secure</router-link>
        </div>
        <router-view />
    </div>
</template>

<script>
import { mapMutations } from "vuex";
import AuthService from "@/auth/AuthService";
import AuthUser from "@/auth/AuthUser";

export default {
    async created() {
        const authService = new AuthService(this.tokenExpiredCallback, "/logincallback");
        const user = await authService.getUser();
        if (user && user.isAuthenticated) {
            this.setUser(user);
            console.info("Logged In: ", user.name, user.bemsId);
        } else {
            console.info("No user initially logged in.");
        }
    },
    methods: {
        ...mapMutations("auth", ["setUser"]),
        tokenExpiredCallback() {
            this.setUser(new AuthUser());
        }
    }
};
</script>

<style lang="scss">
body {
    margin: 0;
}
#app {
    font-family: "Avenir", Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
}
#nav {
    background: rgb(255, 255, 255);
    background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(255, 255, 255, 1) 90%, rgba(232, 232, 232, 1) 100%);
    padding: 10px 0;
    margin: 0 auto 30px;
    display: grid;
    grid-template-columns: 400px 100px 100px;

    h4 {
        margin: 0;
    }
    a {
        padding: 0 1rem;
        font-weight: bold;
        color: #2c3e50;
        &.router-link-exact-active {
            color: #42b983;
        }
    }
}
</style>
