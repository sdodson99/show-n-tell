import ImagePost from "../models/image-post";

class APIRandomImagePostService {
  /**
   * Initialize with a base url.
   * @param {string} baseUrl The base url of the API (not including ending /)
   */
  constructor(baseUrl, apiClient) {
    this.baseUrl = baseUrl;
    this.apiClient = apiClient;
  }

  /**
   * Get a random image post from the API.
   */
  async getRandom() {
    const url = `${this.baseUrl}/imageposts/random`;

    const apiResponse = await this.apiClient.fetch(url);

    if(apiResponse.status === 404) {
      return null
    }

    const imagePostResponse = await apiResponse.json();

    return ImagePost.fromJSON(imagePostResponse);
  }
}

export default APIRandomImagePostService;
