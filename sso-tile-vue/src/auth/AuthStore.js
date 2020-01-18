import AuthUser from "@/auth/AuthUser";

export default {
    namespaced: true,
    state: {
        user: new AuthUser()
    },
    mutations: {
        setUser(state, newUser) {
            state.user = newUser;
        }
    }
};
