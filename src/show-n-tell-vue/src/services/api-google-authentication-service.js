import User from '../models/user'

export const AuthenticationEvents = {
    LOGIN: "login",
    LOGOUT: "logout",
}

class APIGoogleAuthenticationService{
    constructor(baseUrl, tokenService, userService) {
        this.baseUrl = baseUrl
        this.tokenService = tokenService
        this.userService = userService
        this.subscribers = {}
    }

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

        this.publish(AuthenticationEvents.LOGIN, true)

        return user;
    }

    logout() {
        const success = this.userService.clearUser() && this.tokenService.clearToken() 

        if(success) {
            this.publish(AuthenticationEvents.LOGOUT, false)
        }

        return success
    }

    getUser() {
        return this.userService.getUser()
    }

    isLoggedIn() {
        return this.userService.getUser() !== null && this.tokenService.getToken() !== null
    }

    subscribe(event, callback) {
        if(!this.subscribers[event]) {
            this.subscribers[event] = []
        }

        this.subscribers[event].push(callback)

        const callbackIndex = this.subscribers[event].length - 1
        return () => {
            return this.subscribers[event].splice(callbackIndex, 1)
        }
    }

    publish(event, data) {
        if(this.subscribers[event]) {
            this.subscribers[event].forEach(callback => {
                callback(data)
            })
        }
    }
}

export default APIGoogleAuthenticationService