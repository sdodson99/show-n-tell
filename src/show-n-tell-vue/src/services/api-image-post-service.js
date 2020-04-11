import ImagePost from '../models/image-post'

/**
 * Service to perform CRUD on image posts.
 */
class APIImagePostService {
    constructor(baseUrl, apiClient) {
      this.baseUrl = baseUrl;
      this.apiClient = apiClient;
    }

    /**
     * Get an image post by id.
     * @param {number} id The image post id.
     * @returns {ImagePost} The image post matching the id. Null if image post does not exist.
     */
    async getById(id) {
        const url = `${this.baseUrl}/imageposts/${id}`;
  
        const apiResponse = await this.apiClient.fetch(url)

        if(apiResponse.status === 404) {
            return null
        }

        const imagePostResponse = await apiResponse.json()

        return ImagePost.fromJSON(imagePostResponse)
    }
  
    /**
     * Create a new image post.
     * @param {Object} imagePost The image post to create.
     * @returns {ImagePost} The created image post.
     */
    async create(imagePost) {
        const url = `${this.baseUrl}/imageposts/`;
  
        // Create form data for the new image post.
        const formData = new FormData()
        formData.append('image', imagePost.image)
        formData.append('description', imagePost.description)
        formData.append('tags', imagePost.tags.join(','))

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'POST',
            body: formData
        })

        const imagePostResponse = await apiResponse.json()
        
        return ImagePost.fromJSON(imagePostResponse);
    }

    /**
     * Update an image post by id.
     * @param {number} imagePostId The id of the image post.
     * @param {Object} imagePost The updated image post data.
     * @returns {ImagePost} The updated image post.
     */
    async update(imagePostId, imagePost) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(imagePost)
        })

        const imagePostResponse = await apiResponse.json()

        return ImagePost.fromJSON(imagePostResponse);
    }

    /**
     * Delete an image post.
     * @param {number} imagePostId 
     * @returns {boolean} True/false for success.
     */
    async delete(imagePostId) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}`

        const result = await this.apiClient.authFetch(url, {
            method: 'DELETE'
        })

        return result.ok;
    }
  }
  
  export default APIImagePostService;
  