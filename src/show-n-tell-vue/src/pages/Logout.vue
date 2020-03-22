<template>
  <div class="text-center">Logging out...</div>
</template>

<script>
export default {
  name: "Logout",
  props: {
    authenticationService: Object
  },
  created: function() {
    this.authenticationService.logout();
    window.location.href = "/";
  }
};
</script>

<style></style>
