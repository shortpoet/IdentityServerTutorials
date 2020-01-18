<template>
    <div class="secure">
        <h1>The Secure Page</h1>
        <p>If you can see this content, then congratulations, you have been successfullly authenticated!</p>
        <!--
    Wondering how you got here?
    router.js has `requiresAuth` as metadata for this route.
    main.js defines a `beforeEach` nav guard that looks for the `requiresAuth` attribute.
    When the route `requiresAuth` and the user isn't authenticated, the hook forwards you to /login
    /login invokes the OIDC-Client, which eventually drops you off at /logincallback.
    /logincallback sets your auth user data into `$store.state.auth.user` and forwards back to the original URL.
        -->
    </div>
</template>
