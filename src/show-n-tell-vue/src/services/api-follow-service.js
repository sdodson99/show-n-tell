import Following from '../models/following'

/**
 * Service to follow and unfollow users.
 */
class APIFollowService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Follow a user.
     * @param {string} username The username of the user to follow.
     * @returns {Follow} The follow response.
     */
    async follow(username) {
        const url = `${this.baseUrl}/profiles/${username}/follow`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'POST'
        })

        const followResponse = await apiResponse.json();

        return Following.fromJSON(followResponse)
    }

    /**
     * Unfollow a user.
     * @param {string} username The username of the user to unfollow.
     * @returns {boolean} True/false for success.
     */
    async unfollow(username) {
        const url = `${this.baseUrl}/profiles/${username}/follow`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'DELETE'
        })

        return apiResponse.ok
    }
}

export default APIFollowService