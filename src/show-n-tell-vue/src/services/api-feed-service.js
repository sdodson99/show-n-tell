import ImagePost from '../models/image-post'

class APIFeedService {
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async getFeed() {
        const url = `${this.baseUrl}/feed`

        const apiResponse = await this.apiClient.authFetch(url)

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APIFeedService