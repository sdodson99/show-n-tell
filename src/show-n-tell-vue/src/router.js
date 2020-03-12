import Vue from "vue";
import VueRouter from "vue-router";
import ServiceContainer from "./services/service-container";

const TITLE_SUFFIX = " - Show 'N Tell";

Vue.use(VueRouter);

const router = new VueRouter({
  routes: [
    {
      path: "/",
      alias: "/home",
      meta: {
        title: "Home"
      },
      component: () => import("./components/Home")
    },
    {
      path: "/explore",
      meta: {
        title: "Explore"
      },
      component: () => import("./pages/Explore"),
      props: {
        randomImagePostService: ServiceContainer.RandomImagePostService
      }
    },
    {
      path: "/feed",
      meta: {
        title: "Feed"
      },
      component: () => import("./components/Feed")
    },
    {
      path: "/login",
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
    }
  ]
});

router.beforeEach((to, from, next) => {
  document.title = to.meta.title + TITLE_SUFFIX;
  next();
});

export default router;
