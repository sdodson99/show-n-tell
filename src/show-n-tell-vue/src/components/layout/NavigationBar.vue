<template>
  <div id="nav-root">
    <div class="container">
      <nav class="row p-3 text-center">
        <router-link
          class="col-lg-auto mx-1 text-lg-left row justify-content-center link"
          to="/"
        >
          <div class="content px-3 p-2">Show 'N Tell</div>
        </router-link>
        <div class="col"></div>
        <router-link
          class="col-lg-auto mx-1 row justify-content-center link hover-link"
          active-class="active-link"
          to="/explore"
        >
          <div class="content px-3 p-2">Explore</div>
        </router-link>
        <router-link
          v-if="isLoggedIn"
          class="col-lg-auto mx-1 row justify-content-center link hover-link"
          active-class="active-link"
          to="/create"
        >
          <div class="content px-3 p-2">Create</div>
        </router-link>
        <router-link
          v-if="isLoggedIn"
          class="col-lg-auto mx-1 row justify-content-center link hover-link"
          active-class="active-link"
          to="profile"
        >
          <div class="content px-3 p-2">Profile</div>
        </router-link>
        <router-link
          v-if="!isLoggedIn"
          class="col-lg-auto mx-1 row justify-content-center link hover-link"
          active-class="active-link"
          to="login"
        >
          <div class="content px-3 p-2">Login</div>
        </router-link>
        <router-link
          v-if="isLoggedIn"
          class="col-lg-auto mx-1 row justify-content-center link hover-link"
          active-class="active-link"
          to="logout"
        >
          <div class="content px-3 p-2">Logout</div>
        </router-link>
      </nav>
    </div>
  </div>
</template>

<script>
export default {
  name: "NavigationBar",
  props: {
    isLoggedIn: Boolean
  }
};
</script>

<style>
#nav-root {
  background: var(--color-grayscale-light);
  color: var(--color-primary-dark);
  font-size: 1.5em;
  border-bottom: 1px solid var(--color-primary-dark);
}

.link {
  text-decoration: none;
  color: inherit;
  cursor: pointer;
}

.link .content {
  min-width: 100px;
  border-bottom: 3px solid var(--color-grayscale-light);
}

.active-link .content {
  border-bottom: 3px solid var(--color-secondary-medium);
}

.hover-link .content:hover {
  border-bottom: 3px solid var(--color-secondary-dark);
}
</style>
