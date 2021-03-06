import { Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction, Mutation as ImagePostsMutation } from '../image-posts/types'

export { ModuleName } from './types'

export default function createSearchModule(searchService) {
    const state = {
        imagePostIds: []
    }

    const getters = {
        imagePosts: (state, g, rootState) => state.imagePostIds.map(id => rootState.imagePosts.imagePosts[id])
    }

    const actions = {
        async [Action.SEARCH_IMAGE_POSTS]({ commit }, query) {
            const imagePosts = await searchService.searchImagePosts(query)

            commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, imagePosts, { root: true })
            commit(Mutation.SET_IMAGE_POST_IDS, imagePosts.map(p => p.id))
        },
        async [Action.DELETE_IMAGE_POST]({ commit, dispatch, rootState }, imagePostId) {
            await dispatch(`${ImagePostsModuleName}/${ImagePostsAction.DELETE_IMAGE_POST}`, imagePostId, { root: true })

            if(!rootState.imagePosts.imagePosts[imagePostId]) {
                commit(Mutation.REMOVE_IMAGE_POST_FROM_SEARCH, imagePostId)
            }
        }
    }

    const mutations = {
        [Mutation.SET_IMAGE_POST_IDS]: (state, imagePostIds) => state.imagePostIds = imagePostIds,
        [Mutation.REMOVE_IMAGE_POST_FROM_SEARCH]: (state, imagePostId) => {
            state.imagePostIds = state.imagePostIds.filter(x => x !== imagePostId)
        }
    }

    return {
        namespaced: true,
        state,
        getters,
        actions,
        mutations
    }
}