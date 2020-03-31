import ImagePost from '../models/image-post'

class APISearchService {
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async searchImagePosts(searchQuery){
        const url = `${this.baseUrl}/imageposts?search=${searchQuery}`

        const apiResponse = await this.apiClient.fetch(url)

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APISearchService