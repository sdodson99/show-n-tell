class APIProfileService{
    constructor(baseUrl) {
        this.baseUrl = baseUrl
    }

    async getImagePosts(username) {
        const url = `${this.baseUrl}/profiles/${username}/imageposts`

        const apiResponse = await fetch(url);

        return await apiResponse.json();
    }
}

export default APIProfileService