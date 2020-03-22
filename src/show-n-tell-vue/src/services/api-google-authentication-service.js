import User from '../models/user'

class APIGoogleAuthenticationService{
    constructor(baseUrl, tokenService, userService) {
        this.baseUrl = baseUrl
        this.tokenService = tokenService
        this.userService = userService
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

        return user;
    }

    logout() {
        return this.userService.clearUser() && this.tokenService.clearToken() 
    }

    isLoggedIn() {
        return this.userService.getUser() !== null && this.tokenService.getToken() !== null
    }
}

export default APIGoogleAuthenticationService