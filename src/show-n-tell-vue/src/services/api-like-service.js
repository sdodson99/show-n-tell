class APILikeService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    async likeImagePost(id) {
        const url = `${this.baseUrl}/imageposts/${id}/like`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "POST"
        });        

        return await apiResponse.json();
    }

    async unlikeImagePost(id) {
        const url = `${this.baseUrl}/imageposts/${id}/like`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "DELETE"
        });

        return await apiResponse.ok
    }
}

export default APILikeService