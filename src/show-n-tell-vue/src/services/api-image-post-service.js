class APIImagePostService {
    /**
     * Initialize with a base url.
     * @param {string} baseUrl The base url of the API (not including ending /)
     */
    constructor(baseUrl, apiClient) {
      this.baseUrl = baseUrl;
      this.apiClient = apiClient;
    }

    /**
     * Get an image post from the API by id.
     */
    async getById(id) {
        const url = `${this.baseUrl}/imageposts/${id}`;
  
        // Make the API request.
        const result = await this.apiClient.fetch(url)

        const imagePost = await result.json()

        // Convert the date to JS.
        if(imagePost.dateCreated) {
            imagePost.dateCreated = new Date(imagePost.dateCreated)
        }

        return imagePost
    }
  
    /**
     * Post an image post to the API.
     */
    async create(imagePost) {
        const url = `${this.baseUrl}/imageposts/`;
  
        // Create form data for the new image post.
        const formData = new FormData()
        formData.append('image', imagePost.image)
        formData.append('description', imagePost.description)

        // Make the post request.
        const result = await this.apiClient.authFetch(url, {
            method: 'POST',
            body: formData
        })

        return await result.json();
    }

    /**
     * Update an image post by id.
     * @param {Number} imagePostId 
     * @param {Object} imagePost 
     */
    async update(imagePostId, imagePost) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}`

        const result = await this.apiClient.authFetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(imagePost)
        })

        return await result.json();
    }

    /**
     * Delete an image post.
     * @param {Number} imagePostId 
     */
    async delete(imagePostId) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}`

        // Make the delete request.
        const result = await this.apiClient.authFetch(url, {
            method: 'DELETE'
        })

        return result.ok;
    }
  }
  
  export default APIImagePostService;
  