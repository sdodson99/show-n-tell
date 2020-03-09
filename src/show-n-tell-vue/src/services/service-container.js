import AuthenticationService from './local-storage-authentication-service'

const ServiceContainer = {
    AuthenticationService: new AuthenticationService()
}

export default ServiceContainer