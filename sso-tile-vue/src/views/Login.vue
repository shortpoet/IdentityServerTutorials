<template>
    <h1>Logging in...</h1>
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
        authService
            .login(this.$route.query.returnTo)
            .then(user => {
                // Will return immediately on our way to OAuth
                if (!user.isAuthenticated) return;

                this.setUser(user);
                console.info("Grabbed login info from local data: ", user.name, user.bemsId);

                let returnPath;
                try {
                    returnPath = this.$route.query.returnTo || "/";
                } catch (e) {
                    returnPath = "/";
                }
                this.$router.push(returnPath);
            })
            .catch(er => {
                console.warn("This Broke");
                console.error(er);
            });
    }
};
</script>