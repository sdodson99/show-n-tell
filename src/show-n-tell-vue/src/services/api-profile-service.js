import ImagePost from '../models/image-post'
import Profile from '../models/profile'

/**
 * Service to retrieve user profile information.
 */
class APIProfileService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Get a user's profile.
     * @param {string} username The profile username.
     * @returns {Profile} The profile for the user. Null if no profile exists.
     */
    async getProfile(username) {
        const url = `${this.baseUrl}/profiles/${username}`

        const apiResponse = await this.apiClient.authFetch(url);

        const profileResponse = await apiResponse.json()

        return Profile.fromJSON(profileResponse)
    }

    /**
     * Get image posts for a profile.
     * @param {string} username The profile username.
     * @returns {Array} The profile's image posts.
     */
    async getImagePosts(username) {
        const url = `${this.baseUrl}/profiles/${username}/imageposts`

        const apiResponse = await this.apiClient.authFetch(url);

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APIProfileService