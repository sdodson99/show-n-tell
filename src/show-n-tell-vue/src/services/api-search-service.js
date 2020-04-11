import ImagePost from '../models/image-post'

/**
 * Service to search for image posts.
 */
class APISearchService {
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Search for image posts matching a search query.
     * @param {string} searchQuery The search query phrase.
     * @returns {Array} The image posts matching the search query.
     */
    async searchImagePosts(searchQuery){
        const url = `${this.baseUrl}/imageposts?search=${searchQuery}`

        const apiResponse = await this.apiClient.fetch(url)

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APISearchService