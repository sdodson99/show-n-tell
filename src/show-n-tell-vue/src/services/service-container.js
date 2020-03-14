import LocalStorageTokenService from "./local-storage-token-service";
import APIGoogleAuthenticationService from "./api-google-authentication-service";
import APIRandomImagePostService from "./api-random-image-post-service";
import APIImagePostService from "./api-image-post-service";

const baseUrl = "https://localhost:5001";
const tokenService = new LocalStorageTokenService();

const ServiceContainer = {
  AuthenticationService: new APIGoogleAuthenticationService(baseUrl, tokenService),
  TokenService: tokenService,
  RandomImagePostService: new APIRandomImagePostService(baseUrl),
  ImagePostService: new APIImagePostService(baseUrl)
};

export default ServiceContainer;
