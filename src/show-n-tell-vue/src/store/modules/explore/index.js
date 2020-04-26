import { Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction, Mutation as ImagePostsMutation } from '../image-posts/types'

export default function createExploreModule(imagePostService, randomImagePostService) {
    const state = {
        imagePostIds: [],
        currentImagePostIndex: 0,
        noImagePostsAvailable: false,
        imagePostNotFound: false,
        isLoading: false
    }

    const getters = {
        currentImagePostId: (state) => state.imagePostIds[state.currentImagePostIndex],
        currentImagePost: (s, getters, rootState) => rootState.imagePosts.imagePosts[getters.currentImagePostId],
        noImagePosts: (state) => state.imagePostIds.length === 0,
        canExplore: (state) => !state.noImagePostsAvailable,
        isShowingLastImage: (state) => state.currentImagePostIndex + 1 === state.imagePostIds.length,
        hasPreviousImagePost: (state) => state.currentImagePostIndex > 0,
        currentImagePostRoute: (s, getters) => `/explore/${getters.currentImagePost.id}`
    }

    const actions = {
        async [Action.FETCH_RANDOM_IMAGE_POST]({ commit }) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const randomImagePost = await randomImagePostService.getRandom();

                if(randomImagePost && randomImagePost.id) {
                    commit(Mutation.ADD_IMAGE_POST_ID, randomImagePost.id)
                    commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [randomImagePost], { root: true })
                }
            } catch (error) {
                commit(Mutation.SET_HAS_NO_IMAGE_POSTS, true)
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.FETCH_IMAGE_POST_BY_ID]({ commit }, id) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const imagePost = await imagePostService.getById(id);
                
                if(imagePost && imagePost.id) {
                    commit(Mutation.ADD_IMAGE_POST_ID, imagePost.id)
                    commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost], { root: true })
                }
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
        },
        [Action.PREVIOUS_IMAGE_POST]({ commit, getters }) {
            if(getters.hasPreviousImagePost) {
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex - 1)
            }
        },
        async [Action.DELETE_IMAGE_POST]({ commit, getters, dispatch }) {
            if(await imagePostService.delete(getters.currentImagePostId)) {
                commit(Mutation.REMOVE_IMAGE_POST_ID, getters.currentImagePostId)

                // Get an initial image if length is 0.
                if(state.imagePostIds.length === 0) {
                    await dispatch(Action.FETCH_RANDOM_IMAGE_POST)
                }

                // Coerce current image index to the last image available if index larger than images length.
                if(state.currentImagePostIndex >= state.imagePostIds.length) {
                    commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.imagePostIds.length - 1)
                }
            }
        }
    }

    const mutations = {
        [Mutation.ADD_IMAGE_POST_ID]: (state, imagePostId) => state.imagePostIds.push(imagePostId),
        [Mutation.REMOVE_IMAGE_POST_ID]: (state, id) => state.imagePostIds = state.imagePostIds.filter(x => x !== id),
        [Mutation.CLEAR_IMAGE_POST_IDS]: (state) => state.imagePostIds = [],
        [Mutation.SET_CURRENT_IMAGE_POST_INDEX]: (state, currentImagePostIndex) => state.currentImagePostIndex = currentImagePostIndex,
        [Mutation.SET_NO_IMAGE_POSTS_AVAILABLE]: (state, noImagePostsAvailable) => state.noImagePostsAvailable = noImagePostsAvailable,
        [Mutation.SET_IMAGE_POST_NOT_FOUND]: (state, imagePostNotFound) => state.imagePostNotFound = imagePostNotFound,
        [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading
    }

    return {
        namespaced: true,
        state,
        getters,
        actions,
        mutations
    }
}