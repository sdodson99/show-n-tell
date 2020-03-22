<template>
  <div id="nav-root">
    <div class="container">
      <nav :class="{ open: open }" class="row p-3 align-items-center justify-content-center">
        <div class="col-lg-auto d-flex flex-column align-items-center justify-content-center">
          <router-link
            class="row justify-content-center mx-1 link"
            to="/"
          >
            <img id="logo" class="ml-3 ml-lg-0" src="../../assets/showntell.png"/>
          </router-link>
          <Hamburger id="ham" class="d-lg-none mt-3" 
            @toggleOn="opened" @toggleOff="closed" :toggled="open"
            toggle-on-style="stroke: var(--color-secondary-medium)"
            toggle-off-style="stroke: var(--color-primary-dark)"></Hamburger>
        </div>
        <div class="d-none d-lg-block col"/>
        <div id="menu-items" :class="{ open: open }" class="col-lg-auto row align-items-center">
          <router-link
            class="col-lg-auto row mx-1 justify-content-center text-center link hover-link"
            active-class="active-link"
            to="/explore"
          >
            <div class="content px-3 p-2">Explore</div>
          </router-link>
          <router-link
            v-if="isLoggedIn"
            class="col-lg-auto row mx-1 justify-content-center text-center link hover-link"
            active-class="active-link"
            to="/create"
          >
            <div class="content px-3 p-2">Create</div>
          </router-link>
          <router-link
            v-if="isLoggedIn"
            class="col-lg-auto row mx-1 justify-content-center text-center link hover-link"
            active-class="active-link"
            to="/profile"
          >
            <div class="content px-3 p-2">Profile</div>
          </router-link>
          <router-link
            v-if="!isLoggedIn"
            class="col-lg-auto row mx-1 justify-content-center text-center link hover-link"
            active-class="active-link"
            to="/login"
          >
            <div class="content px-3 p-2">Login</div>
          </router-link>
          <router-link
            v-if="isLoggedIn"
            class="col-lg-auto row mx-1 justify-content-center text-center link hover-link"
            active-class="active-link"
            to="/logout"
          >
            <div class="content px-3 p-2">Logout</div>
          </router-link>
        </div>
      </nav>
    </div>
  </div>
</template>

<script>
import Hamburger from './Hamburger'

export default {
  name: "NavigationBar",
  components: {
    Hamburger
  },
  props: {
    isLoggedIn: Boolean
  },
  data: function(){
    return {
      open: false
    }
  },
  methods: {
    opened: function() {
      this.open = true;
    },
    closed: function() {
      this.open = false;
    }
  }
};
</script>

<style scoped>
#ham{
  height: 1.5em;
  width: 1.5em;
}

#logo {
  max-height: 75px;
  max-width: 100%;
}

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

#menu-items {
  max-height: 0;
  transition: max-height 1s;
  overflow: hidden;
}

#menu-items.open {
  max-height: 500px;
}

@media screen and (min-width: 992px) {
  #menu-items, #menu-items.open {
    max-height: 500px;
  }
}
</style>
