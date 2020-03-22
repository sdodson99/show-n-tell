import ImagePost from '../models/image-post'

class APIProfileService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async getImagePosts(username) {
        const url = `${this.baseUrl}/profiles/${username}/imageposts`

        const apiResponse = await this.apiClient.fetch(url);

        const imagePostsResponse = await apiResponse.json()

        return imagePostsResponse.map(p => ImagePost.fromJSON(p))
    }
}

export default APIProfileService