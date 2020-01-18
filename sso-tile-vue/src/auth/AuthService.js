import { UserManager, WebStorageStateStore } from "oidc-client";
import AuthUser from "./AuthUser";

/**
 * Auth Service. A simpler interface to the Authentication Service and a place to apply an Authorization Service.
 */
export class AuthService {
    /**
     * AuthService constructor
     * @param router React-router-dom instance from props.history
     */
    constructor(tokenExpiredCallback, callbackUrl) {
        this._authentication = new AuthenticationService(tokenExpiredCallback, callbackUrl);
    }

    /**
     * The starting place for a login
     *
     * Danger: This will return an AuthUser or a Promise that redirects. You've been warned!
     *
     * @param {string} [returnPath="/"] URL that the login attempt is coming from, relative to the domain, e.g. `/admin`
     * @returns if not authenticated, redirect to SSO login
     * @returns {AuthUser} if already authenticated, a populated AuthUser
     */
    async login(returnPath) {
        let user = await this.getUser(returnPath);
        if (!user.isAuthenticated) {
            let ssoUser = await this._authentication.login(returnPath);
            if (ssoUser && ssoUser.profile && ssoUser.profile.user_name) {
                user = this.getAuthUserFromSsoUser(ssoUser);
            } else {
                user = new AuthUser();
            }
        }
        return user;
    }

    /**
     * Get a logged in user without the possibility of a forward
     * @returns { AuthUser } An AuthUser, check if isAuthenticated
     */
    async getUser(returnPath) {
        if (await this._authentication.isAuthenticated()) {
            const ssoUser = await this._authentication.getAuthenticatedUser(returnPath);
            return this.getAuthUserFromSsoUser(ssoUser);
        } else {
            return new AuthUser();
        }
    }

    /**
     * Process the HTTP callback and finish the authentication trip as well as authorization for this app
     * @returns { AuthUser } An AuthUser, likely to be populated if authentication was successful
     */
    async signInCallback() {
        const ssoUser = await this._authentication.signinRedirectCallback();
        return this.getAuthUserFromSsoUser(ssoUser);
    }

    /**
     * Transform an SSO Auth login + Profile seciton into a simple, local AuthUser obejct for appliation use
     * @returns { AuthUser } A populated AuthUser
     */
    getAuthUserFromSsoUser(ssoUser) {
        var user = new AuthUser();
        user.bemsId = ssoUser.profile.user_name;
        user.email = ssoUser.profile.email;
        user.name = ssoUser.profile.given_name + " " + ssoUser.profile.family_name;
        user.token = ssoUser.id_token;
        user.isAuthenticated = user.bemsId ? true : false;
        return user;
    }

    async getToken() {
        return await this._authentication.getExistingToken();
    }
}
export default AuthService;

export class AuthenticationService {
    /**
     * Authentication Service constructor
     * @param {function} tokenExpiredCallback Method to call when the token expires
     */
    constructor(tokenExpiredCallback, callbackUrl) {
        this._tokenExpiredCallback = tokenExpiredCallback;
        this._callbackUrl = callbackUrl;
    }

    async getUserManager(returnPath) {
        let returnTo = returnPath ? returnPath : "/";
        let clientRoot = window.location.protocol + "//" + window.location.host;

        if (!this._oidcSettings) {
            let ssoData = this.getSsoEnvironment();
            let settings = {
                authority: ssoData.clientUrl,
                client_id: ssoData.clientId,
                redirect_uri: `${clientRoot}${this._callbackUrl}?returnTo=${encodeURIComponent(returnTo)}`,
                post_logout_redirect_uri: `${clientRoot}/`,
                response_type: "code id_token",
                scope: "openid profile",
                userStore: new WebStorageStateStore({ store: localStorage })
            };
            this._oidcSettings = settings;
        }

        // reset the returnTo attribute because if it gets cached, we get redirected incorrectly
        this._oidcSettings.redirect_uri = `${clientRoot}${this._callbackUrl}?returnTo=${encodeURIComponent(returnTo)}`;

        let userManager = new UserManager(this._oidcSettings);
        if (this._tokenExpiredCallback) {
            userManager.events.addAccessTokenExpired(this._tokenExpiredCallback);
        }
        return userManager;
    }

    /**
     * Looks for an existing user (however OIDC-Connect stores it).
     * @returns bool, true if the token exists and is valid
     */
    async isAuthenticated() {
        let oidcUser = await this.getAuthenticatedUser();
        return oidcUser !== null && oidcUser.expired === false;
    }

    /**
     * Gets an existing authenticated user, or else a null
     */
    async getAuthenticatedUser(returnPath = "/") {
        let userManager = await this.getUserManager(returnPath);
        return await userManager.getUser();
    }

    /**
     * Gets the SSO Tile environment data. Could be async if this data is on a server (and it should be)
     * FIXME: put your own data or access to the data in here!
     * @returns {object} in the format: `{"clientId":"36-char-uuid","clientUrl":"auth url"}`
     */
    getSsoEnvironment() {
        // The regular testing sso config
        return {
            clientId: "8c68bd2d-0a10-4c4c-86bf-1f6c35213406",
            clientUrl: "https://apps-bemsid-only.login.system.pcfpre-phx.cloud.boeing.com"
        };

        //this one has mostly the defaults
        // return {
        //     clientId: "6fc8512e-c56f-4cd7-87b5-4deb75766af4",
        //     clientUrl: "https://apps-bemsid-only.login.system.pcfpre-phx.cloud.boeing.com"
        // };
    }

    /**
     * Starting place for a fresh SSO login attempt
     * @param returnTo URL that the login attempt is coming returnTo, relative to the domain, e.g. `/tooljail` @default `/`
     * @returns a Promise, but redirects the browser to the SSO login page.
     */
    async login(returnTo) {
        let userManager = await this.getUserManager(returnTo);
        return userManager.signinRedirect();
    }

    /**
     * The OIDC-Client sign-in, in this case "callback" was at the http level as the user is returned from their SSO Tile sign-in
     * @returns {Promise<OidcUser>} a UserManager().getUser() object with tokens and all
     */
    async signinRedirectCallback(returnTo) {
        let userManager = await this.getUserManager(returnTo);
        return await userManager.signinRedirectCallback();
    }

    async getExistingToken() {
        let userManager = await this.getUserManager();
        let user = await userManager.getUser();
        return user.id_token;
    }
}

window.as = new AuthService();
