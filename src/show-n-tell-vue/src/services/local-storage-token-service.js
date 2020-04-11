/**
 * Service for storing the current user's access token.
 */
class LocalStorageTokenService {
  constructor() {
    this.accessTokenKey = "accessToken"
  }

  /**
   * Get the user's access token.
   * @returns {string} The access token.
   */
  getToken() {
    return window.localStorage.getItem(this.accessTokenKey);
  }

  /**
   * Store the user's access token.
   * @param {string} token The access token.
   * @returns {boolean} True/false for success.
   */
  setToken(token) {
    window.localStorage.setItem(this.accessTokenKey, token);
    return true;
  }

  /**
   * Clear the user's stored access token.
   * @returns {boolean} True/false for success.
   */
  clearToken() {
    window.localStorage.removeItem(this.accessTokenKey);
    return true;
  }
}

export default LocalStorageTokenService;
