import User from '../models/user'

/**
 * Constants for authentication event names.
 */
export const AuthenticationEvents = {
    LOGIN: "login",
    LOGOUT: "logout",
}

/**
 * Service for authenticating a Google login token with Show 'N Tell.
 */
class APIGoogleAuthenticationService{
    constructor(baseUrl, tokenService, userService) {
        this.baseUrl = baseUrl
        this.tokenService = tokenService
        this.userService = userService
    }

    /**
     * Login a user with a Google token.
     * @param {string} token The Google token to login.
     * @returns {User} The logged in user.
     */
    async login(token) {
        const url = `${this.baseUrl}/auth/google`

        const apiResponse = await fetch(url, {
            method: "POST",
            headers: {
                'Authorization': 'bearer ' + token
            }
        });

        const userResponse = await apiResponse.json();
        const user = User.fromJSON(userResponse)
        
        this.tokenService.setToken(token)
        this.userService.setUser(user)

        return user;
    }

    /**
     * Logout the current user.
     * @returns {boolean} True/false for success.
     */
    logout() {
        this.userService.clearUser()
        this.tokenService.clearToken() 

        return true
    }

    /**
     * Get the current logged in user.
     * @returns {User} The logged in user. Null if the user is not logged in.
     */
    getUser() {
        return this.userService.getUser()
    }

    /**
     * Check if a user is logged in.
     * @returns {boolean} True/false for success.
     */
    isLoggedIn() {
        return this.userService.getUser() !== null && this.tokenService.getToken() !== null
    }
}

export default APIGoogleAuthenticationService