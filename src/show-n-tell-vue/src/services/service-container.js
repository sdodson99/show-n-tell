import LocalStorageTokenService from "./local-storage-token-service";
import LocalStorageUserService from "./local-storage-user-service";
import APIGoogleAuthenticationService from "./api-google-authentication-service";
import APIRandomImagePostService from "./api-random-image-post-service";
import APIImagePostService from "./api-image-post-service";
import APIProfileService from "./api-profile-service";
import APIClient from "./authentication-api-client"

const baseUrl = "https://localhost:5001";
const tokenService = new LocalStorageTokenService();
const userService = new LocalStorageUserService();
const apiClient = new APIClient(tokenService, userService);

const ServiceContainer = {
  AuthenticationService: new APIGoogleAuthenticationService(baseUrl, tokenService, userService),
  TokenService: tokenService,
  UserService: userService,
  RandomImagePostService: new APIRandomImagePostService(baseUrl, apiClient),
  ImagePostService: new APIImagePostService(baseUrl, apiClient),
  ProfileService: new APIProfileService(baseUrl, apiClient)
};

export default ServiceContainer;
