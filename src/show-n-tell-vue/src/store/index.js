import Vuex from 'vuex';
import Vue from 'vue';

import router from '../router'
import ServiceContainer from '../services/service-container'

import LikeVueService from "../services/vue-services/like-vue-service";
import CommentVueService from "../services/vue-services/comment-vue-service";

import createAuthenticationModule from './modules/authentication'
import createExploreModule from './modules/explore'
import createCreateModule from './modules/create';
import createEditModule from './modules/edit';
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
        explore: createExploreModule(ServiceContainer.ImagePostService, ServiceContainer.RandomImagePostService, likeVueService, commentVueService),
        create: createCreateModule(ServiceContainer.ImagePostService, router),
        edit: createEditModule(ServiceContainer.ImagePostService, router),
        feed: createFeedModule(ServiceContainer.FeedService, likeVueService, commentVueService, router),
        profile: createProfileModule(ServiceContainer.ProfileService, ServiceContainer.FollowService, ServiceContainer.ImagePostService, likeVueService, router),
        search: createSearchModule(ServiceContainer.SearchService, likeVueService, router)
    }
})