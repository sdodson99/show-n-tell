import Like from '../models/like'

/**
 * Service to like and unlike image posts.
 */
class APILikeService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Like an image post.
     * @param {number} id The image post id.
     * @returns {Like} The like response.
     */
    async likeImagePost(id) {
        const url = `${this.baseUrl}/imageposts/${id}/like`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "POST"
        });        

        const likeResponse = await apiResponse.json()

        return Like.fromJSON(likeResponse);
    }

    /**
     * Unlike an image post.
     * @param {number} id The image post id.
     * @returns {boolean} True/false for success.
     */
    async unlikeImagePost(id) {
        const url = `${this.baseUrl}/imageposts/${id}/like`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "DELETE"
        });

        return await apiResponse.ok
    }
}

export default APILikeService