import LocalStorageAuthenticationService from "./local-storage-authentication-service";
import APIRandomImagePostService from "./api-random-image-post-service";
import APIImagePostService from "./api-image-post-service";

const baseUrl = "https://localhost:5001";

const ServiceContainer = {
  AuthenticationService: new LocalStorageAuthenticationService(),
  RandomImagePostService: new APIRandomImagePostService(baseUrl),
  ImagePostService: new APIImagePostService(baseUrl)
};

export default ServiceContainer;
