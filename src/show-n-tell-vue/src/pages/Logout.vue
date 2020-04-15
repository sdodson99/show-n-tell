<template>
  <div>
    <div class="text-center">Logging out...</div>
    <div class="text-center">
      <b-spinner class="mt-4" label="Logging out..."></b-spinner>
    </div>
  </div>
</template>

<script>
export default {
  name: "Logout",
  props: {
    authenticationService: Object
  },
  created: function() {
    this.authenticationService.logout();
    this.$router.push({path: "/"})
  }
};
</script>

<style></style>
