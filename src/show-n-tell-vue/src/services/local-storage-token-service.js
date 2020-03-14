class LocalStorageTokenService {
  constructor() {
    this.accessTokenKey = "accessToken"
  }

  getToken() {
    return window.localStorage.getItem(this.accessTokenKey);
  }

  setToken(token) {
    window.localStorage.setItem(this.accessTokenKey, token);
    return true;
  }

  clearToken() {
    window.localStorage.removeItem(this.accessTokenKey);
    return true;
  }
}

export default LocalStorageTokenService;
