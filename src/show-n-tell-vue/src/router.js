import Vue from "vue";
import VueRouter from "vue-router";

import ServiceContainer from "./services/service-container";

const TITLE_SUFFIX = " - Show 'N Tell";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history"
})

router.addRoutes([
  {
    name: "Home",
    path: "/",
    redirect: "/explore",
  },
  {
    path: "/explore/:initialId?",
    meta: {
      title: "Explore"
    },
    component: () => import("./pages/Explore"),
  },
  {
    path: "/feed",
    meta: {
      title: "Feed",
      authenticate: true
    },
    component: () => import("./pages/Feed")
  },
  {
    path: "/search",
    meta: {
      title: "Search"
    },
    component: () => import("./pages/Search"),
    props: (route) => {
      return {
        query: route.query.q
    }
    }
  },
  {
    path: "/create",
    meta: {
      title: "Create",
      authenticate: true
    },
    component: () => import("./pages/Create")
  },
  {
    path: "/image-posts/:imagePostId/edit",
    meta: {
      title: "Edit",
      authenticate: true
    },
    component: () => import("./pages/Edit")
  },
  {
    path: "/profile",
    meta: {
      title: "Profile"
    },
    beforeEnter: (to, from, next) => {
      const currentUser = ServiceContainer.AuthenticationService.getUser()

      if(currentUser) {
        next({path: `/profile/${currentUser.username}`})
      } else {
        next({name: "Login"})
      }
    },
  },
  {
    path: "/profile/:username",
    meta: {
      title: "Profile"
    },
    component: () => import("./pages/Profile")
  },
  {
    path: "/login",
    name: "Login",
    meta: {
      title: "Login"
    },
    component: () => import("./pages/Login"),
    props: (route) => {
      return {
        redirectPath: route.query.redirect,
        redirectBack: route.query.back
      }
    }
  },
  {
    name: "Logout",
    path: "/logout",
    meta: {
      title: "Logout"
    },
    component: () => import("./pages/Logout")
  }, 
  {
    path: "*",
    redirect: "/"
  }
])

// Restrict authenticated routes.
router.beforeEach((to, from, next) => {
  if(to.meta.authenticate && !ServiceContainer.AuthenticationService.isLoggedIn()) {
    next({name: "Login"})
  } else if(to.name === "Login" && ServiceContainer.AuthenticationService.isLoggedIn()) {
    next({name: "Home"})
  } else {
    next()
  }
});

// Set document title.
router.beforeEach((to, from, next) => {
  document.title = to.meta.title + TITLE_SUFFIX;
  next();
});

export default router;
