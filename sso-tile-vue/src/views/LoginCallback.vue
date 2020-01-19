<template>
    <h1>Processing login...</h1>
</template>

<script>
import AuthService from "@/auth/AuthService";
import { mapMutations } from "vuex";

export default {
    methods: {
        ...mapMutations("auth", ["setUser"])
    },
    mounted() {
        const authService = new AuthService(() => true, "/logincallback");
        authService.signInCallback().then(user => {
            if (!user.isAuthenticated) return;

            this.setUser(user);
            console.info("Ran through SSO Tile and logged in: ", user.name, user.userName);

            let returnPath;
            try {
                returnPath = this.$route.query.returnTo || "/";
            } catch (e) {
                returnPath = "/";
            }
            this.$router.push(returnPath);
        });
    }
};
</script>