import { Action, Mutation } from './types'

import ServiceContainer from '../../../services/service-container'

const imagePostService = ServiceContainer.ImagePostService;
const randomImagePostService = ServiceContainer.RandomImagePostService;

const state = {
    imagePosts: [],
    currentImagePostIndex: 0,
    noImagePostsAvailable: false,
    imagePostNotFound: false,
    isLoading: false
}

const getters = {
    currentImagePost: (state) => state.imagePosts[state.currentImagePostIndex],
    canExplore: (state) => !state.noImagePostsAvailable,
    isShowingLastImage: (state) => state.currentImagePostIndex + 1 === state.imagePosts.length,
    hasPreviousImagePost: (state) => state.currentImagePostIndex > 0
}

const actions = {
    async [Action.FETCH_RANDOM_IMAGE_POST]({ commit }) {
        commit(Mutation.SET_IS_LOADING, true)

        try {
            const randomImagePost = await randomImagePostService.getRandom();

            let existingImagePost = state.imagePosts.find(p => p.id === randomImagePost.id)
    
            commit(Mutation.ADD_IMAGE_POST, existingImagePost || randomImagePost)
        } catch (error) {
            commit(Mutation.SET_HAS_NO_IMAGE_POSTS, true)
        }

        commit(Mutation.SET_IS_LOADING, false)
    },
    async [Action.FETCH_IMAGE_POST_BY_ID]({ commit }, id) {
        commit(Mutation.SET_IS_LOADING, true)

        try {
            const imagePost = await imagePostService.getById(id);
            
            commit(Mutation.ADD_IMAGE_POST, imagePost)
        } catch (error) {
            commit(Mutation.SET_IMAGE_POST_NOT_FOUND, true)
            commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, -1)
        }

        commit(Mutation.SET_IS_LOADING, false)
    },
    async [Action.NEXT_IMAGE_POST]({ commit, getters, dispatch }) {
        if(getters.isShowingLastImage) {
            await dispatch(Action.FETCH_RANDOM_IMAGE_POST);
        }

        commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex + 1)
    } ,
    [Action.PREVIOUS_IMAGE_POST]({ commit, getters }) {
        if(getters.hasPreviousImagePost) {
            commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex - 1)
        }
    }
}

const mutations = {
    [Mutation.ADD_IMAGE_POST]: (state, imagePost) => state.imagePosts.push(imagePost),
    [Mutation.SET_CURRENT_IMAGE_POST_INDEX]: (state, currentImagePostIndex) => state.currentImagePostIndex = currentImagePostIndex,
    [Mutation.SET_NO_IMAGE_POSTS_AVAILABLE]: (state, noImagePostsAvailable) => state.noImagePostsAvailable = noImagePostsAvailable,
    [Mutation.SET_IMAGE_POST_NOT_FOUND]: (state, imagePostNotFound) => state.imagePostNotFound = imagePostNotFound,
    [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading,
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}