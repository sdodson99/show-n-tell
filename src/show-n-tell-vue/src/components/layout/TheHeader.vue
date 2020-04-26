<template>
  <div id="nav-root">
    <div class="container">
      <nav class="d-flex flex-column flex-lg-row p-3 align-items-center justify-content-center">
        <div class="col-lg-auto d-flex flex-column align-items-center justify-content-center">
          <router-link
            class="row justify-content-center mx-1 link"
            to="/">
            <img id="logo" class="ml-3 ml-lg-0" src="../../assets/showntell.png"/>
          </router-link>
          <Hamburger id="ham" class="d-lg-none mt-3" 
            v-b-toggle.menu-items
            toggle-on-style="stroke: var(--color-secondary-medium)"
            toggle-off-style="stroke: var(--color-primary-dark)"></Hamburger>
        </div>
        <b-collapse id="menu-items" class="flex-grow-1 align-items-center justify-content-lg-end">
          <form ref="searchForm" id="search-form" class="d-flex flex-grow-1 mx-4 my-2"
            @submit.prevent="search">
            <input id="search-input" class="form-control flex-grow-1"
              v-model="searchContent"
              type="search" placeholder="Search images..." aria-label="Search"
              required/>
            <button id="search-button" type="button" @click="search">Search</button>
          </form>

          <router-link class="row justify-content-center text-center link hover-link" active-class="active-link"
            v-for="menuItem in menuItems"
            :key="menuItem.id"
            :to="menuItem.to">
            <div class="content px-1 p-2">{{ menuItem.name }}</div>
          </router-link>
        </b-collapse>
      </nav>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

import Hamburger from '../utilities/Hamburger'

export default {
  name: "TheHeader",
  components: {
    Hamburger
  },
  data: function(){
    return {
      searchContent: ""
    }
  },
  computed: {
    ...mapGetters('authentication', ['isLoggedIn']),
    menuItems: function() {
      const menuItems = []

      menuItems.push({
        name: "Explore",
        to: "/explore"
      })

      if(this.isLoggedIn) {
        menuItems.push(
        {
          name: "Feed",
          to: "/feed"
        },
        {
          name: "Create",
          to: "/create"
        },
        {
          name: "Profile",
          to: "/profile"
        },
        {
          name: "Logout",
          to: "/logout"
        })
      } else {
        menuItems.push(
        {
          name: "Login",
          to: "/login"
        })
      }

      return menuItems
    }
  },
  methods: {
    search: function() {
      if(this.$refs.searchForm.reportValidity()) {
        this.$router.push({path: "/search", query: { q: this.searchContent }})
      }
    }
  }
};
</script>

<style scoped>
#ham{
  height: 1.5em;
  width: 1.5em;
  cursor: pointer;
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
  margin: 0 10px;
}

.content {
  font-size: 0.75em;
}

.link .content {
  min-width: 75px;
  border-bottom: 3px solid var(--color-grayscale-light);
}

.active-link .content {
  border-bottom: 3px solid var(--color-secondary-medium);
}

.hover-link .content:hover {
  border-bottom: 3px solid var(--color-secondary-dark);
}

#search-form {
  max-width: 300px;
}

#search-input {
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
  box-shadow: none;
  overflow: hidden;
}

#search-button {
  margin: 0;
  padding: 2px 10px;
  min-width: auto;
  font-size: 0.75em;
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
}

@media screen and (min-width: 992px) {
  #menu-items{
    display: flex !important;
  }
}
</style>
