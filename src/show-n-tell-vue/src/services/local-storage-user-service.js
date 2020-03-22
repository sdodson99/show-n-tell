import User from "../models/user";

class LocalStorageUserService{
    constructor(){
        this.userKey = "user"
    }

    getUser() {
        let user = window.localStorage.getItem(this.userKey);

        if(user !== null) {
            const userResponse = JSON.parse(user)
            return User.fromJSON(userResponse)
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