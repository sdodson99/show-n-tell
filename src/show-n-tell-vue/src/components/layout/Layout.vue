<template>
  <div>
    <NavigationBar v-bind:is-logged-in="isLoggedIn" />
    <div v-if="statusMessage" :class="alertClass" class="alert text-center">
      {{ statusMessage }}
    </div>
    <div class="container">
      <router-view class="m-5"
        @alert-success="alertSuccess"
        @alert-status="alertStatus"
        @alert-error="alertError"></router-view>
    </div>
  </div>
</template>

<script>
import NavigationBar from "./NavigationBar";

export default {
  name: "Layout",
  components: {
    NavigationBar
  },
  props: {
    isLoggedIn: Boolean
  },
  data: function() {
    return {
      statusMessage: "",
      alertClass: "" || "alert-primary",
      timeoutHandle: 0
    }
  },
  methods: {
    alertSuccess: function(message, duration) {
      this.alertClass = "alert-success"
      this._setMessage(message, duration)
    },
    alertStatus: function(message, duration) {
      this.alertClass = "alert-primary"
      this._setMessage(message, duration)
    },
    alertError: function(message, duration) {
      this.alertClass = "alert-danger"
      this._setMessage(message, duration)

    },
    _setMessage: function(message, duration) {
      this.statusMessage = message
      clearTimeout(this.timeoutHandle)
      this.timeoutHandle = setTimeout(() => this.statusMessage = "", duration || 3000)
    }
  }
};
</script>

<style></style>
