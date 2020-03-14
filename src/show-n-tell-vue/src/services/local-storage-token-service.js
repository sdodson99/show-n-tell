class LocalStorageTokenService {
  getToken() {
    return window.localStorage.getItem("accessToken");
  }

  saveToken(token) {
    window.localStorage.setItem("accessToken", token);
    return true;
  }

  clearToken() {
    window.localStorage.removeItem("accessToken");
    return true;
  }
}

export default LocalStorageTokenService;
