<template>
  <div>Logging out...</div>
</template>

<script>
import AuthenticationService from "../services/local-storage-authentication-service";

export default {
  name: "Logout",
  props: {
    authenticationService: AuthenticationService
  },
  created: function() {
    this.authenticationService.logout();
    window.location.href = "/";
  }
};
</script>

<style></style>
