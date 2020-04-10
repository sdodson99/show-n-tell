import Vue from "vue";
import VueRouter from "vue-router";

import ServiceContainer from "./services/service-container";
import LikeVueService from "./services/vue-services/like-vue-service";
import CommentVueService from "./services/vue-services/comment-vue-service";

const TITLE_SUFFIX = " - Show 'N Tell";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history"
})

// Create services that depend on router.
const likeVueService = new LikeVueService(ServiceContainer.LikeService, ServiceContainer.AuthenticationService, router)
const commentVueService = new CommentVueService(ServiceContainer.CommentService, ServiceContainer.AuthenticationService, router)

router.addRoutes([
  {
    path: "/",
    name: "Home",
    redirect: "/explore",
  },
  {
    path: "/feed",
    meta: {
      title: "Feed",
      authenticate: true
    },
    component: () => import("./pages/Feed"),
    props: {
      userService: ServiceContainer.UserService,
      likeVueService: likeVueService,
      commentVueService: commentVueService,
      feedService: ServiceContainer.FeedService
    }
  },
  {
    path: "/search",
    meta: {
      title: "Search"
    },
    component: () => import("./pages/Search"),
    props: (route) => {
      return {
        query: route.query.q,
        searchService: ServiceContainer.SearchService,
        imagePostService: ServiceContainer.ImagePostService,
        likeVueService: likeVueService,
        userService: ServiceContainer.UserService
    }
    }
  },
  {
    path: "/explore/:initialId?",
    meta: {
      title: "Explore"
    },
    component: () => import("./pages/Explore"),
    props: {
      imagePostService: ServiceContainer.ImagePostService,
      randomImagePostService: ServiceContainer.RandomImagePostService,
      likeVueService: likeVueService,
      commentVueService: commentVueService,
      userService: ServiceContainer.UserService
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
    path: "/imageposts/:imagePostId/edit",
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
    component: () => import("./pages/Profile"),
    props: {
      imagePostService: ServiceContainer.ImagePostService,
      profileService: ServiceContainer.ProfileService,
      followService: ServiceContainer.FollowService,
      likeVueService: likeVueService,
      userService: ServiceContainer.UserService
    }
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
        authenticationService: ServiceContainer.AuthenticationService,
        redirect: route.query.redirect,
        back: route.query.back
      }
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
])

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
