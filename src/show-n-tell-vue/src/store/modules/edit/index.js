import { Action, Mutation } from './types'

import UnauthorizedError from '../../../errors/unauthorized-error'

export default function createEditStore(imagePostService, router) {

    const state = {
        imagePostId: 0,
        imageUri: "",
        description: "",
        tags: "",
        isLoading: false,
        isUpdating: false
    }

    const actions = {
        async [Action.SET_IMAGE_POST_BY_ID]({ commit }, id) {
            commit(Mutation.SET_IS_LOADING, true)

            const imagePost = await imagePostService.getById(id)
            commit(Mutation.SET_IMAGE_POST_ID, id)
            commit(Mutation.SET_IMAGE_URI, imagePost.imageUri)
            commit(Mutation.SET_DESCRIPTION, imagePost.description)
            commit(Mutation.SET_TAGS, imagePost.tags.join(','))

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.UPDATE_IMAGE_POST]({ commit }) {
            commit(Mutation.SET_IS_UPDATING, true)

            const imagePost = {
                description: state.description,
                tags: state.tags.split(",").map(t => t.trim())
            }

            try {
                const updatedImagePost = await imagePostService.update(state.imagePostId, imagePost)

                if(updatedImagePost.id) {
                    router.push({path: `/explore/${updatedImagePost.id}`})
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }

            commit(Mutation.SET_IS_UPDATING, false)
        }
    }

    const mutations = {
        [Mutation.SET_IMAGE_POST_ID]: (state, imagePostId) => state.imagePostId = imagePostId,
        [Mutation.SET_IMAGE_URI]: (state, imageUri) => state.imageUri = imageUri,
        [Mutation.SET_DESCRIPTION]: (state, description) => state.description = description,
        [Mutation.SET_TAGS]: (state, tags) => state.tags = tags,
        [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading,
        [Mutation.SET_IS_UPDATING]: (state, isUpdating) => state.isUpdating = isUpdating
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}