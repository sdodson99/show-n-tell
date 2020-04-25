import { Action, Mutation } from './types'

import UnauthorizedError from '../../../errors/unauthorized-error'

export default function createFeedModule(feedService, likeVueService, commentVueService, router) {
    const state = {
        imagePosts: [],
        isLoading: false
    }

    const getters = {

    }

    const actions = {
        async [Action.GET_FEED]({ commit }) {
            commit(Mutation.SET_IS_LOADING, true)

            try {  
                const imagePosts = await feedService.getFeed()
                commit(Mutation.SET_IMAGE_POSTS, imagePosts)
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.LIKE_IMAGE_POST]({ commit }, imagePost) {
            const newLike = await likeVueService.likeImagePost(imagePost)
            if(newLike) {
                commit(Mutation.ADD_LIKE_TO_IMAGE_POST, { imagePostId: imagePost.id, like: newLike })
            }
        },
        async [Action.UNLIKE_IMAGE_POST]({ commit}, imagePost) {
            const removedLike = await likeVueService.unlikeImagePost(imagePost)
            if(removedLike) {
                commit(Mutation.REMOVE_LIKE_FROM_IMAGE_POST, { imagePostId: imagePost.id, like: removedLike });
            }
        },
        async [Action.CREATE_IMAGE_POST_COMMENT]({ commit }, { imagePost, content}) {
            const createdComment = await commentVueService.createComment(imagePost, content, '/feed')
            if(createdComment) {
                commit(Mutation.ADD_COMMENT_TO_IMAGE_POST, { imagePostId: imagePost.id, comment: createdComment })
            }
        },
        async [Action.UPDATE_IMAGE_POST_COMMENT]({ commit }, { imagePost, comment}) {
            const updatedComment = await commentVueService.updateComment(imagePost, comment.id, comment.content, '/feed')
            if(updatedComment) {
                commit(Mutation.UPDATE_COMMENT_ON_IMAGE_POST, { imagePostId: imagePost.id, comment: updatedComment })
            }
        },
        async [Action.DELETE_IMAGE_POST_COMMENT]({ commit }, {imagePost, commentId}) {
            const deletedComment = await commentVueService.deleteComment(imagePost, commentId, '/feed')
            if(deletedComment) {
                commit(Mutation.REMOVE_COMMENT_FROM_IMAGE_POST, { imagePostId: imagePost.id, commentId })
            }
        },
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
        [Mutation.ADD_COMMENT_TO_IMAGE_POST]: (state, { imagePostId, comment}) => {
            const imagePostIndex = state.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.imagePosts[imagePostIndex].comments.push(comment)
            }
        },
        [Mutation.UPDATE_COMMENT_ON_IMAGE_POST]: (state, { imagePostId, comment}) => {
            const imagePostIndex = state.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                const commentIndex = state.imagePosts[imagePostIndex].comments.findIndex(c => c.id === comment.id)
                if(commentIndex !== -1) {
                    state.imagePosts[imagePostIndex].comments.splice(commentIndex, 1, comment)
                }
            }
        },
        [Mutation.REMOVE_COMMENT_FROM_IMAGE_POST]: (state, { imagePostId, commentId}) => {
            const imagePostIndex = state.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.imagePosts[imagePostIndex].comments = state.imagePosts[imagePostIndex].comments.filter(c => c.id !== commentId)
            }
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