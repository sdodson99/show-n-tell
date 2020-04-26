<template>
  <div class="d-flex flex-column align-items-center">
    <h1>Login</h1>
    <div v-if="!isLoggingIn" class="d-flex flex-column align-items-center">
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
import { mapState } from 'vuex'
import { ModuleName, Action } from '../store/modules/authentication/types'
import GoogleLogin from "vue-google-login";

export default {
  name: "Login",
  components: {
    GoogleLogin
  },
  props: {
    redirectPath: String,
    redirectBack: Boolean
  },
  data() {
    return {
      isLoggingIn: false,
      googleParams: {
        client_id:
          "462162693296-pesibunbs71up7lress4c6b549bd2qld.apps.googleusercontent.com"
      },
      googleRenderParams: {
        width: 150,
        height: 50
      }
    };
  },
  methods: {
    onLoginSuccess: async function(result) {
      this.isLoggingIn = true

      const token = result.getAuthResponse().id_token;
      await this.$store.dispatch(`${ModuleName}/${Action.LOGIN}`, {
        token,
        redirectPath: this.redirectPath,
        redirectBack: this.redirectBack
      })

      this.isLoggingIn = false
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
