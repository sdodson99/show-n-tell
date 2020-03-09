<template>
    <div class="d-flex flex-column align-items-center">
        <h1>Login</h1>
        <p class="my-3 font-weight-light">Click below to login to your Show 'N Tell account with Google.</p>
        <GoogleLogin class="my-3" :params="googleParams" :renderParams="googleRenderParams" 
            :onSuccess="onLoginSuccess" 
            :onFailure="onLoginFailure">Login</GoogleLogin>
    </div>
</template>

<script>
import GoogleLogin from 'vue-google-login';
import AuthenticationService from '../services/local-storage-authentication-service';

export default {
    name: "Login",
    props: {
        authenticationService: AuthenticationService
    },
    data() {
        return {
            googleParams: {
                client_id: "462162693296-pesibunbs71up7lress4c6b549bd2qld.apps.googleusercontent.com"
            },
            googleRenderParams: {
                width: 150,
                height: 50
            }
        }
    },
    components: {
        GoogleLogin
    },
    methods: {
        onLoginSuccess: function(result){
            const accessToken = result.getAuthResponse().id_token

            if(this.authenticationService.login(accessToken)){
                window.location.href = "/"
            }
        },
        onLoginFailure: function(e){
            console.log(e);
        }
    }
}
</script>

<style>
    h1{
        color: var(--color-primary-dark);
    }
</style>