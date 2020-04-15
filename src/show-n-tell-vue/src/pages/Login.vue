<template>
  <div class="d-flex flex-column align-items-center">
    <h1>Login</h1>
    <div v-if="!isLoading" class="d-flex flex-column align-items-center">
      <p class="my-3 font-weight-light text-center">
        Click below to login to your Show 'N Tell account with Google.
      </p>
      <GoogleLogin class="my-3"
        :params="googleParams"
        :renderParams="googleRenderParams"
        :onSuccess="onLoginSuccess"
        :onFailure="onLoginFailure">Login</GoogleLogin>
    </div>
    <div v-else class="text-center">
      <b-spinner class="mt-4" label="Logging in..."></b-spinner>
    </div>
  </div>
</template>

<script>
import GoogleLogin from "vue-google-login";

export default {
  name: "Login",
  props: {
    authenticationService: Object,
    redirect: String,
    back: Boolean
  },
  data() {
    return {
      googleParams: {
        client_id:
          "462162693296-pesibunbs71up7lress4c6b549bd2qld.apps.googleusercontent.com"
      },
      googleRenderParams: {
        width: 150,
        height: 50
      },
      isLoading: false
    };
  },
  components: {
    GoogleLogin
  },
  methods: {
    onLoginSuccess: async function(result) {
      this.isLoading = true

      const accessToken = result.getAuthResponse().id_token;

      if (await this.authenticationService.login(accessToken)) {
        if(this.redirect) {
          this.$router.push({path: this.redirect})
        } else if (this.back) {
          this.$router.go(-1)
        } else {
          this.$router.push({path: "/"})
        }
      }

      this.isLoading = false
    },
    onLoginFailure: function(e) {
      console.log(e);
    }
  }
};
</script>

<style>

.abcRioButtonContents span{
  display: none;
}

.abcRioButtonContents::after{
  content: "Sign in";
}

</style>
