<template>
  <div>
    <div class="text-center">Logging out...</div>
    <div class="text-center">
      <b-spinner class="mt-4" label="Logging out..."></b-spinner>
    </div>
  </div>
</template>

<script>
import { ModuleName, Action } from '../store/modules/authentication/types'

export default {
  name: "Logout",
  created: function() {
    this.$store.dispatch(`${ModuleName}/${Action.LOGOUT}`)
  }
};
</script>