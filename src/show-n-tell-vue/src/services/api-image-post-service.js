class APIImagePostService {
    /**
     * Initialize with a base url.
     * @param {string} baseUrl The base url of the API (not including ending /)
     */
    constructor(baseUrl) {
      this.baseUrl = baseUrl;
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
  