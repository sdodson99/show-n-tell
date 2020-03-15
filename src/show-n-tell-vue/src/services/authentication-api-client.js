import UnauthorizedError from '../errors/unauthorized-error'

class AuthenticationAPIClient{
    constructor(tokenService, userService){
        this.tokenService = tokenService;
        this.userService = userService;
    }

    async fetch(url, options) {
        const response = await fetch(url, options)

        if(response.status === 401) {
            this.tokenService.clearToken()
            this.userService.clearUser()

            throw new UnauthorizedError();
        }

        return response;
    }

    async authFetch(url, options) {
        options = this._prepareOptions(options)

        options.headers.append("Authorization", `Bearer ${this.tokenService.getToken()}`)

        return await this.fetch(url, options);
    }

    _prepareOptions(options) {
        if(!options) {
            options = {}
        }

        options.headers = new Headers(options.headers)

        return options
    }
}

export default AuthenticationAPIClient