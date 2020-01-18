<template>
    <div class="home">
        <h1>Home Page</h1>
        <p>You found your way to the home page. Great job.</p>
        <pre>
Authenticated: {{ user.isAuthenticated }}
Email Address: {{ user.email }}
BEMS ID: {{ user.bemsId }}
Name: {{ user.name }}</pre>
    </div>
</template>

<script>
import { mapState } from "vuex";

export default {
    name: "home",
    computed: {
        ...mapState("auth", ["user"])
    }
};
</script>

<style lang="scss" scoped="true">
pre {
    width: 400px;
    margin: 0 auto;
    padding: 1rem;
    background-color: #fff3cd;
    border-radius: 5px;
    text-align: left;
}
</style>