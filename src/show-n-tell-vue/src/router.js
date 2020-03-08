import Vue from "vue";
import VueRouter from "vue-router";

const TITLE_SUFFIX = " - Show 'N Tell"

Vue.use(VueRouter);

const router = new VueRouter({
  routes: [
    {
      path: "/",
      alias: "/home",
      meta: {
        title: "Home" + TITLE_SUFFIX
      },
      component: () => import("./components/Home")
    },
    {
      path: "/explore",
      meta: {
        title: "Explore" + TITLE_SUFFIX
      },
      component: () => import("./components/Explore")
    },
    {
      path: "/feed",
      meta: {
        title: "Feed" + TITLE_SUFFIX
      },
      component: () => import("./components/Feed")
    }
  ]
});

router.beforeEach((to, from, next) => {
  document.title = to.meta.title;
  next();
});

export default router
