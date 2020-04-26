<template>
  <div>
    <the-header/>
    <div v-if="statusMessage" :class="alertClass" class="alert text-center">
      {{ statusMessage }}
    </div>
    <div class="container">
      <router-view class="my-5 mx-3"/>
    </div>
  </div>
</template>

<script>
import TheHeader from "./TheHeader";

export default {
  name: "Layout",
  components: {
    TheHeader
  },
  data: function() {
    return {
      statusMessage: "",
      alertClass: "" || "alert-primary",
      timeoutHandle: 0
    }
  },
  mounted: function(){
    this.$el.addEventListener('alert-success', this.alertSuccess)
    this.$el.addEventListener('alert-status', this.alertStatus)
    this.$el.addEventListener('alert-error', this.alertError)
  },
  methods: {
    alertSuccess: function(e) {
      this.alertClass = "alert-success"
      this._setMessage(e)
    },
    alertStatus: function(e) {
      this.alertClass = "alert-primary"
      this._setMessage(e)
    },
    alertError: function(e) {
      this.alertClass = "alert-danger"
      this._setMessage(e)
    },
    _setMessage: function(e) {
      const message = e.detail.message
      const duration = e.detail.duration || 3000
      
      this.statusMessage = message
      clearTimeout(this.timeoutHandle)
      this.timeoutHandle = setTimeout(() => this.statusMessage = "", duration)
    }
  }
};
</script>
