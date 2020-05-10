import { Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../image-posts/types'

import UnauthorizedError from '../../../errors/unauthorized-error'

export { ModuleName } from './types'

export default function createFeedModule(feedService, router) {
    const state = {
        imagePostIds: []
    }

    const getters = {
        imagePosts: (state, g, rootState) => state.imagePostIds.map(id => rootState.imagePosts.imagePosts[id])
    }

    const actions = {
        async [Action.GET_FEED]({ commit }) {
            try {  
                const imagePosts = await feedService.getFeed()
                
                commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, imagePosts, { root: true})
                commit(Mutation.SET_IMAGE_POST_IDS, imagePosts.map(p => p.id))
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }
        }
    }

    const mutations = {
        [Mutation.SET_IMAGE_POST_IDS]: (state, imagePostIds) => state.imagePostIds = imagePostIds,
        [Mutation.ADD_IMAGE_POST_ID_TO_BEGINNING]: (state, imagePostId) => { 
            if(state.imagePostIds.every(p => p.id !== imagePostId)) {
                state.imagePostIds.unshift(imagePostId)
            }
        },
        [Mutation.REMOVE_IMAGE_POST_ID]: (state, imagePostId) => {
            state.imagePostIds = state.imagePostIds.filter(p => p !== imagePostId)
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