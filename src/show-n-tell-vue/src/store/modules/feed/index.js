import { Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../image-posts/types'

import UnauthorizedError from '../../../errors/unauthorized-error'

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
        [Mutation.SET_IMAGE_POST_IDS]: (state, imagePostIds) => state.imagePostIds = imagePostIds
    }

    return {
        namespaced: true,
        state,
        getters,
        actions,
        mutations
    }
}