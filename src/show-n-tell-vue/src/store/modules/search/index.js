import { Action, Mutation } from './types'

export default function createSearchModule(searchService, imagePostService, likeVueService) {
    const state = {
        imagePosts: [],
        isLoading: false
    }

    const actions = {
        async [Action.SEARCH_IMAGE_POSTS]({ commit }, query) {
            commit(Mutation.SET_IS_LOADING, true)

            const imagePosts = await searchService.searchImagePosts(query)
            commit(Mutation.SET_IMAGE_POSTS, imagePosts)

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.LIKE_IMAGE_POST]({ commit }, imagePost) {
            const newLike = await likeVueService.likeImagePost(imagePost)
            if(newLike) {
                commit(Mutation.ADD_LIKE_TO_IMAGE_POST, { imagePostId: imagePost.id, like: newLike })
            }
        },
        async [Action.UNLIKE_IMAGE_POST]({ commit }, imagePost) {
            const removedLike = await likeVueService.unlikeImagePost(imagePost)
            if(removedLike) {
                commit(Mutation.REMOVE_LIKE_FROM_IMAGE_POST, { imagePostId: imagePost.id, like: removedLike });
            }
        },
        async [Action.DELETE_IMAGE_POST]({ commit }, imagePostId) {
            if(await imagePostService.delete(imagePostId)) {
                commit(Mutation.REMOVE_IMAGE_POST_FROM_SEARCH, imagePostId)
            }
        }
    }

    const mutations = {
        [Mutation.SET_IMAGE_POSTS]: (state, imagePosts) => state.imagePosts = imagePosts,
        [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading,
        [Mutation.ADD_LIKE_TO_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePostIndex = state.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.imagePosts[imagePostIndex].likes.push(like)
            }
        },
        [Mutation.REMOVE_LIKE_FROM_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePostIndex = state.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.imagePosts[imagePostIndex].likes = state.imagePosts[imagePostIndex].likes.filter(l => l.userEmail !== like.userEmail)
            }
        },
        [Mutation.REMOVE_IMAGE_POST_FROM_SEARCH]: (state, imagePostId) => {
            state.imagePosts = state.imagePosts.filter(p => p.id !== imagePostId)
        }
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}