import Vuex from 'vuex';
import Vue from 'vue';

import router from '../router'
import ServiceContainer from '../services/service-container'

import LikeVueService from "../services/vue-services/like-vue-service";
import CommentVueService from "../services/vue-services/comment-vue-service";

import createAuthenticationModule from './modules/authentication'
import createImagePostsModule from './modules/image-posts'
import createExploreModule from './modules/explore'
import createFeedModule from './modules/feed';
import createProfileModule from './modules/profile';
import createSearchModule from './modules/search';

Vue.use(Vuex)

// Create services that depend on router.
const likeVueService = new LikeVueService(ServiceContainer.LikeService, ServiceContainer.AuthenticationService, router)
const commentVueService = new CommentVueService(ServiceContainer.CommentService, ServiceContainer.AuthenticationService, router)

export default new Vuex.Store({
    modules: {
        authentication: createAuthenticationModule(ServiceContainer.AuthenticationService, router),
        imagePosts: createImagePostsModule(ServiceContainer.ImagePostService, likeVueService, commentVueService, router),
        explore: createExploreModule(ServiceContainer.ImagePostService, ServiceContainer.RandomImagePostService),
        feed: createFeedModule(ServiceContainer.FeedService, router),
        profile: createProfileModule(ServiceContainer.ProfileService, ServiceContainer.FollowService, ServiceContainer.ImagePostService, router),
        search: createSearchModule(ServiceContainer.SearchService, ServiceContainer.ImagePostService, likeVueService, router)
    }
})