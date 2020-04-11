import User from "../models/user";

/**
 * Service for the current application user.
 */
class LocalStorageUserService{
    constructor(){
        this.userKey = "user"
    }

    /**
     * Get the current application user.
     * @returns {User} The current user. Null if no user is set.
     */
    getUser() {
        let user = window.localStorage.getItem(this.userKey);

        if(user !== null) {
            const userResponse = JSON.parse(user)
            return User.fromJSON(userResponse)
        } else {
            return null
        }
    }

    /**
     * Set the current application user.
     * @param {User} user The current user.
     * @returns {boolean} True/false for success.
     */
    setUser(user) {
        window.localStorage.setItem(this.userKey, JSON.stringify(user))
        return true
    }

    /**
     * Clear the current application user.
     */
    clearUser() {
        window.localStorage.clear(this.userKey)
    }
}

export default LocalStorageUserService