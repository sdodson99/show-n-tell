import Follow from '../models/follow'

class APIFollowService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async follow(username) {
        const url = `${this.baseUrl}/profiles/${username}/follow`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'POST'
        })

        const followResponse = await apiResponse.json();

        return Follow.fromJSON(followResponse)
    }

    async unfollow(username) {
        const url = `${this.baseUrl}/profiles/${username}/follow`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'DELETE'
        })

        return apiResponse.ok
    }
}

export default APIFollowService