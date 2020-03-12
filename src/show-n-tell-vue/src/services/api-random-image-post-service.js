class APIRandomImagePostService {
  /**
   * Initialize with a base url.
   * @param {string} baseUrl The base url of the API (not including ending /)
   */
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  /**
   * Get a random image post from the API.
   */
  async getRandom() {
    const url = `${this.baseUrl}/imagepost/random`;

    const apiResponse = await fetch(url);

    let image = await apiResponse.json();
    image.dateCreated = new Date(image.dateCreated);

    return image;
  }
}

export default APIRandomImagePostService;
