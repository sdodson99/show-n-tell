import { Action, Mutation } from './types'

import UnauthorizedError from '../../../errors/unauthorized-error'

export default function createCreateModule(imagePostService, router) {
    const state = {
        isCreating: false
    }

    const actions = {
        async [Action.CREATE_IMAGE_POST]({ commit }, imagePost) {
            commit(Mutation.SET_IS_CREATING, true)

            try {
                const createdImagePost = await imagePostService.create(imagePost);
      
                if(createdImagePost.id) {
                    router.push({path: `/explore/${createdImagePost.id}`})
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }

            commit(Mutation.SET_IS_CREATING, false)
        }
    }

    const mutations = {
        [Mutation.SET_IS_CREATING]: (state, isCreating) => state.isCreating = isCreating
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}