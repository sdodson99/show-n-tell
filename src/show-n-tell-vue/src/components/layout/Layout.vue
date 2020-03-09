<template>
  <div>
    <NavigationBar v-bind:is-logged-in="isLoggedIn"/>
    <div class="container">
      <router-view class="my-5"></router-view>
    </div>
  </div>
</template>

<script>
import NavigationBar from './NavigationBar'

export default {
  name: "Layout",
  components: {
    NavigationBar
  },
  props: {
    isLoggedIn: Boolean
  }
};
</script>

<style></style>