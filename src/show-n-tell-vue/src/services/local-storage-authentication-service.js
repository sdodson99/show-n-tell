class LocalStorageAuthenticationService {
  login(token) {
    window.localStorage.setItem("accessToken", token);
    return true;
  }

  logout() {
    window.localStorage.removeItem("accessToken");
    return true;
  }

  isLoggedIn() {
    return window.localStorage.getItem("accessToken") !== null;
  }
}

export default LocalStorageAuthenticationService;
