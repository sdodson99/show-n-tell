class APIProfileService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async getImagePosts(username) {
        const url = `${this.baseUrl}/profiles/${username}/imageposts`

        const apiResponse = await this.apiClient.fetch(url);

        return await apiResponse.json();
    }
}

export default APIProfileService