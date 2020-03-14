class APIImagePostService {
    /**
     * Initialize with a base url.
     * @param {string} baseUrl The base url of the API (not including ending /)
     */
    constructor(baseUrl) {
      this.baseUrl = baseUrl;
    }

    /**
     * Get an image post from the API by id.
     */
    async getById(id) {
        const url = `${this.baseUrl}/imageposts/${id}`;
  
        // Make the API request.
        const result = await fetch(url)

        const imagePost = await result.json()

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
        const result = await fetch(url, {
            method: 'POST',
            headers: {
                'Authorization': 'bearer ' + localStorage.getItem("accessToken")
            },
            body: formData
        })

        return await result.json();
    }
  }
  
  export default APIImagePostService;
  