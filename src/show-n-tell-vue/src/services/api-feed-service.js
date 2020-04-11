import ImagePost from '../models/image-post'

/**
 * Service to retrieve the user's feed of image posts.
 */
class APIFeedService {
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Get a the current user's feed of image posts.
     * @returns {Array} The list of image posts in the feed.
     */
    async getFeed() {
        const url = `${this.baseUrl}/feed`

        const apiResponse = await this.apiClient.authFetch(url)

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APIFeedService