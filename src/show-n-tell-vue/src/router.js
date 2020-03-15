import Vue from "vue";
import VueRouter from "vue-router";
import ServiceContainer from "./services/service-container";

const TITLE_SUFFIX = " - Show 'N Tell";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "Home",
      redirect: "/explore",
    },
    {
      path: "/explore/:initialId?",
      meta: {
        title: "Explore"
      },
      component: () => import("./pages/Explore"),
      props: {
        imagePostService: ServiceContainer.ImagePostService,
        randomImagePostService: ServiceContainer.RandomImagePostService
      }
    },
    {
      path: "/create",
      meta: {
        title: "Create",
        authenticate: true
      },
      component: () => import("./pages/Create"),
      props: {
        imagePostService: ServiceContainer.ImagePostService
      }
    },
    {
      path: "/imagePosts/:imagePostId/edit",
      meta: {
        title: "Edit",
        authenticate: true
      },
      component: () => import("./pages/Edit"),
      props: {
        imagePostService: ServiceContainer.ImagePostService
      }
    },
    {
      path: "/profile",
      meta: {
        title: "Profile",
        authenticate: true
      },
      component: () => import("./pages/Profile"),
      props: {
        imagePostService: ServiceContainer.ImagePostService,
        userService: ServiceContainer.UserService,
        profileService: ServiceContainer.ProfileService
      }
    },
    {
      path: "/login",
      name: "Login",
      meta: {
        title: "Login"
      },
      component: () => import("./pages/Login"),
      props: {
        authenticationService: ServiceContainer.AuthenticationService
      }
    },
    {
      path: "/logout",
      meta: {
        title: "Logout"
      },
      component: () => import("./pages/Logout"),
      props: {
        authenticationService: ServiceContainer.AuthenticationService
      }
    }, 
    {
      path: "*",
      redirect: "/"
    }
  ]
});

router.beforeEach((to, from, next) => {
  if(to.meta.authenticate && !ServiceContainer.AuthenticationService.isLoggedIn()) {
    next({name: "Login"})
  } else if(to.name === "Login" && ServiceContainer.AuthenticationService.isLoggedIn()) {
    next({name: "Home"})
  } else {
    next()
  }
});

router.beforeEach((to, from, next) => {
  document.title = to.meta.title + TITLE_SUFFIX;
  next();
});

export default router;
