<template>
  <div>
    <NavigationBar/>
    <router-view></router-view>
  </div>
</template>

<script>
import NavigationBar from './NavigationBar'

export default {
  name: "Layout",
  components: {
    NavigationBar
  }
};
</script>

<style></style>