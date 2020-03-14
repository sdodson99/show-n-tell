class APIGoogleAuthenticationService{
    constructor(baseUrl, tokenService) {
        this.baseUrl = baseUrl
        this.tokenService = tokenService
    }

    async login(token) {
        const url = `${this.baseUrl}/auth/google`

        const apiResponse = await fetch(url, {
            method: "POST",
            headers: {
                'Authorization': 'bearer ' + token
            }
        });

        let user = await apiResponse.json();

        console.log(user);

        if(user) {
            this.tokenService.saveToken(token)
        }

        return user;
    }

    logout() {
        return this.tokenService.clearToken()
    }

    isLoggedIn() {
        return this.tokenService.getToken() !== null
    }
}

export default APIGoogleAuthenticationService