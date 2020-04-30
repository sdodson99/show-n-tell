import Vuex from 'vuex';
import Vue from 'vue';

import router from '../router'
import ServiceContainer from '../services/service-container'

import LikeVueService from "../services/vue-services/like-vue-service";
import CommentVueService from "../services/vue-services/comment-vue-service";

import createAuthenticationModule, { ModuleName as AuthenticationModuleName } from './modules/authentication'
import createImagePostsModule, { ModuleName as ImagePostsModuleName } from './modules/image-posts'
import createExploreModule, { ModuleName as ExploreModuleName } from './modules/explore'
import createFeedModule, { ModuleName as FeedModuleName } from './modules/feed';
import createProfileModule, { ModuleName as ProfileModuleName } from './modules/profile';
import createSearchModule, { ModuleName as SearchModuleName } from './modules/search';

Vue.use(Vuex)

// Create services that depend on router.
const likeVueService = new LikeVueService(ServiceContainer.LikeService, ServiceContainer.AuthenticationService, router)
const commentVueService = new CommentVueService(ServiceContainer.CommentService, ServiceContainer.AuthenticationService, router)

const store = new Vuex.Store()

store.registerModule(ImagePostsModuleName, createImagePostsModule(ServiceContainer.ImagePostService, likeVueService, commentVueService, router))
store.registerModule(ExploreModuleName, createExploreModule(ServiceContainer.ImagePostService, ServiceContainer.RandomImagePostService, store))
store.registerModule(FeedModuleName, createFeedModule(ServiceContainer.FeedService, router))
store.registerModule(ProfileModuleName, createProfileModule(ServiceContainer.ProfileService, ServiceContainer.FollowService, router))
store.registerModule(SearchModuleName, createSearchModule(ServiceContainer.SearchService))
store.registerModule(AuthenticationModuleName, createAuthenticationModule(ServiceContainer.AuthenticationService, router))

export default store