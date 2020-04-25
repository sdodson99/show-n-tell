import Vuex from 'vuex';
import Vue from 'vue';

import router from '../router'
import ServiceContainer from '../services/service-container'

import LikeVueService from "../services/vue-services/like-vue-service";
import CommentVueService from "../services/vue-services/comment-vue-service";

import createAuthenticationModule from './modules/authentication'
import createExploreModule from './modules/explore'
import createCreateModule from './modules/create';

Vue.use(Vuex)

// Create services that depend on router.
const likeVueService = new LikeVueService(ServiceContainer.LikeService, ServiceContainer.AuthenticationService, router)
const commentVueService = new CommentVueService(ServiceContainer.CommentService, ServiceContainer.AuthenticationService, router)

export default new Vuex.Store({
    modules: {
        authentication: createAuthenticationModule(ServiceContainer.AuthenticationService, router),
        explore: createExploreModule(ServiceContainer.ImagePostService, ServiceContainer.RandomImagePostService, likeVueService, commentVueService),
        create: createCreateModule(ServiceContainer.ImagePostService, router)
    }
})