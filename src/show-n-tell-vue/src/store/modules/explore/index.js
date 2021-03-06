import { ModuleName, Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction, Mutation as ImagePostsMutation } from '../image-posts/types'

export { ModuleName } from './types'

export default function createExploreModule(imagePostService, randomImagePostService, store) {
    store.subscribe((mutation) => {
        if(mutation.type === `${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_IMAGE_POST}`) {
            const imagePostId = mutation.payload
            store.commit(`${ModuleName}/${Mutation.REMOVE_IMAGE_POST_ID}`, imagePostId)
        }
    })

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
        async [Action.VIEW_RANDOM_IMAGE_POST]({ commit, dispatch }) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const randomImagePost = await randomImagePostService.getRandom();
                dispatch(Action.VIEW_IMAGE_POST, randomImagePost)
            } catch (error) {
                commit(Mutation.SET_NO_IMAGE_POSTS_AVAILABLE, true)
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.VIEW_IMAGE_POST_BY_ID]({ commit, dispatch }, id) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const imagePost = await imagePostService.getById(id);
                dispatch(Action.VIEW_IMAGE_POST, imagePost)
            } catch (error) {
                commit(Mutation.SET_IMAGE_POST_NOT_FOUND, true)
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, -1)
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        [Action.VIEW_IMAGE_POST]({ commit }, imagePost) {            
            if(imagePost && imagePost.id) {
                commit(Mutation.ADD_IMAGE_POST_ID, imagePost.id)                
                commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost], { root: true })
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.imagePostIds.length - 1)
                commit(Mutation.SET_NO_IMAGE_POSTS_AVAILABLE, false)
                commit(Mutation.SET_IMAGE_POST_NOT_FOUND, false)
            }
        },
        async [Action.NEXT_IMAGE_POST]({ commit, getters, dispatch }) {
            if(getters.isShowingLastImage) {
                await dispatch(Action.VIEW_RANDOM_IMAGE_POST);
            } else {
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex + 1)
            }
        },
        [Action.PREVIOUS_IMAGE_POST]({ commit, getters }) {
            if(getters.hasPreviousImagePost) {
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex - 1)
            }
        },
        async [Action.DELETE_IMAGE_POST]({ getters, dispatch, rootState }) {
            await dispatch(`${ImagePostsModuleName}/${ImagePostsAction.DELETE_IMAGE_POST}`, getters.currentImagePostId, { root: true })

            if(!rootState.imagePosts.imagePosts[getters.currentImagePostId]) {
                await dispatch(Action.REMOVE_IMAGE_POST_BY_ID, getters.currentImagePostId)
            }
        },
        async [Action.REMOVE_IMAGE_POST_BY_ID]({ commit, getters, dispatch }, id) {
            commit(Mutation.REMOVE_IMAGE_POST_ID, id)

            // Get an initial image if length is 0.
            if(getters.noImagePosts) {
                await dispatch(Action.VIEW_RANDOM_IMAGE_POST)
            }
        }
    }

    const mutations = {
        [Mutation.ADD_IMAGE_POST_ID]: (state, imagePostId) => state.imagePostIds.push(imagePostId),
        [Mutation.REMOVE_IMAGE_POST_ID]: (state, id) => {
            state.imagePostIds = state.imagePostIds.filter(x => x !== id)

            // Coerce current image index to the last image available if index larger than images length.
            if(state.currentImagePostIndex >= state.imagePostIds.length) {
                state.currentImagePostIndex = state.imagePostIds.length - 1
            }
        },
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