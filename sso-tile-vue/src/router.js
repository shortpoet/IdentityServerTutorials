import Vue from "vue";
import Router from "vue-router";
import Login from "@/views/Login";
import LoginCallback from "@/views/LoginCallback";
import Home from "./views/Home.vue";

Vue.use(Router);

export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        { path: "/login", component: Login },
        { path: "/logincallback", component: LoginCallback },
        {
            path: "/",
            name: "home",
            component: Home
        },
        {
            path: "/secure",
            name: "secure",
            component: () => import(/* webpackChunkName: "secure" */ "./views/Secure.vue"),
            // requiresAuth triggers beforeEach in main.js to make sure the user is authenticated
            meta: { requiresAuth: true }
        }
    ]
});
