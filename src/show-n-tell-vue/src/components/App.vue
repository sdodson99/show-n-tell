<template>
  <div id="app">
    <Layout v-bind:is-logged-in="isLoggedIn" />
  </div>
</template>

<script>
import Layout from "./layout/Layout";

export default {
  name: "App",
  components: {
    Layout
  },
  props: {
    isLoggedIn: Boolean
  }
};
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

a:hover,
a:active,
a:link {
  color: inherit;
  text-decoration: none;
}

#app {
  --color-primary-dark: rgb(49, 60, 71);
  --color-primary-medium: rgb(87, 107, 126);
  --color-primary-light: rgb(134, 164, 199);

  --color-secondary-dark: rgb(241, 158, 13);
  --color-secondary-medium: rgb(236, 182, 89);
  --color-secondary-light: rgb(245, 230, 204);

  --color-grayscale-dark: rgb(53, 53, 53);
  --color-grayscale-medium: rgb(121, 121, 121);
  --color-grayscale-light: rgb(230, 230, 230);
}
</style>
