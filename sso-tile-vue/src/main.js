import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

Vue.config.productionTip = false;

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount("#app");

// Register a global navigation guard for authentication
// Before each route is entered, this checks to see if the user is authenticated.
// If the user is not authenticated but the route requires it, they are forwarded to /login
router.beforeEach((to, from, next) => {
    if (to.meta.requiresAuth && !store.state.auth.user.isAuthenticated) {
        router.push(`/login?returnTo=${to.fullPath}`);
    }
    next();
});
