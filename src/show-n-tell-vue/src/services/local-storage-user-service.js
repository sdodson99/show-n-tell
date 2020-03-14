class LocalStorageUserService{
    constructor(){
        this.userKey = "user"
    }

    getUser() {
        let user = window.localStorage.getItem(this.userKey);

        if(user !== null) {
            return JSON.parse(user)
        } else {
            return null
        }
    }

    setUser(user) {
        window.localStorage.setItem(this.userKey, JSON.stringify(user))
        return true
    }

    clearUser() {
        window.localStorage.clear(this.userKey)
    }
}

export default LocalStorageUserService