import UnauthorizedError from '../errors/unauthorized-error'

/**
 * Make API requests with authentication.
 */
class AuthenticationAPIClient{
    constructor(tokenService, userService){
        this.tokenService = tokenService;
        this.userService = userService;
    }

    /**
     * Make a fetch request.
     * @param {string} url The request url.
     * @param {RequestInit} options Fetch request options.
     * @throws {UnauthorizedError} The current user is unauthorized.
     * @returns The fetch response.
     */
    async fetch(url, options) {
        const response = await fetch(url, options)

        if(response.status === 401) {
            this.tokenService.clearToken()
            this.userService.clearUser()

            throw new UnauthorizedError();
        }

        return response;
    }

    /**
     * Make an authorized fetch request.
     * @param {string} url The request url.
     * @param {RequestInit} options Fetch request options.
     * @throws {UnauthorizedError} The current user is unauthorized.
     * @returns The fetch response.
     */
    async authFetch(url, options) {
        options = this._prepareOptions(options)

        options.headers.append("Authorization", `Bearer ${this.tokenService.getToken()}`)

        return await this.fetch(url, options);
    }

    /**
     * Setup the fetch request options.
     * @param {RequestInit} options Fetch request options.
     * @returns {RequestInit} The initialized fetch request options.
     */
    _prepareOptions(options) {
        if(!options) {
            options = {}
        }

        options.headers = new Headers(options.headers)

        return options
    }
}

export default AuthenticationAPIClient