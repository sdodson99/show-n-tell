import ImagePost from "../models/image-post";

/**
 * Service to retrieve a random image post.
 */
class APIRandomImagePostService {
  constructor(baseUrl, apiClient) {
    this.baseUrl = baseUrl;
    this.apiClient = apiClient;
  }

  /**
   * Get a random image post from the API.
   * @returns {ImagePost} The random image post. Null if no image post found.
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
