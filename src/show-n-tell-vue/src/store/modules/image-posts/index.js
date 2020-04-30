import Vue from 'vue'
import { Action, Mutation } from './types'

import UnauthorizedError from '../../../errors/unauthorized-error'

export { ModuleName } from './types'

export default function createImagePostsModule(imagePostService, likeVueService, commentVueService, router) {
    const state = {
        imagePosts: {}
    }

    const actions = {
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
        async [Action.CREATE_IMAGE_POST_COMMENT]({ commit }, { imagePost, content }) {
            const createdComment = await commentVueService.createComment(imagePost, content)
            if(createdComment) {
                commit(Mutation.ADD_COMMENT_TO_IMAGE_POST, { imagePostId: imagePost.id, comment: createdComment })
            }
        },
        async [Action.UPDATE_IMAGE_POST_COMMENT]({ commit }, { imagePost, comment }) {
            const updatedComment = await commentVueService.updateComment(imagePost, comment.id, comment.content)
            if(updatedComment) {
                commit(Mutation.UPDATE_COMMENT_ON_IMAGE_POST, { imagePostId: imagePost.id, comment: updatedComment })
            }
        },
        async [Action.DELETE_IMAGE_POST_COMMENT]({ commit }, { imagePost, commentId }) {
            const deletedComment = await commentVueService.deleteComment(imagePost, commentId)
            if(deletedComment) {
                commit(Mutation.REMOVE_COMMENT_FROM_IMAGE_POST, { imagePostId: imagePost.id, commentId })
            } 
        },
        async [Action.FETCH_IMAGE_POST_BY_ID]({ commit }, id) {
            const imagePost = await imagePostService.getById(id)
            if(imagePost && imagePost.id) {
                commit(Mutation.UPDATE_IMAGE_POSTS, [imagePost])
            }
        },
        async [Action.CREATE_IMAGE_POST]({ commit }, imagePost) {
            try {
                const createdImagePost = await imagePostService.create(imagePost);
      
                if(createdImagePost.id) {
                    commit(Mutation.UPDATE_IMAGE_POSTS, [createdImagePost])
                    router.push({path: `/explore/${createdImagePost.id}`})
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }
        },
        async [Action.UPDATE_IMAGE_POST]({ commit }, { imagePostId, imagePost}) {
            try {
                const updatedImagePost = await imagePostService.update(imagePostId, imagePost)

                if(updatedImagePost.id) {
                    commit(Mutation.UPDATE_IMAGE_POSTS, [updatedImagePost])
                    router.push({path: `/explore/${updatedImagePost.id}`})
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }
        },
        async [Action.DELETE_IMAGE_POST]({ commit }, imagePostId) {
            if(await imagePostService.delete(imagePostId)) {
                commit(Mutation.REMOVE_IMAGE_POST, imagePostId)
            }
        }
    }

    const mutations = {
        [Mutation.UPDATE_IMAGE_POSTS]: (state, imagePosts) => {
            imagePosts.forEach(p => Vue.set(state.imagePosts, p.id, p))
        },
        [Mutation.ADD_LIKE_TO_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePost = state.imagePosts[imagePostId]
            if(imagePost) {
                imagePost.likes.push(like)
            }
        },
        [Mutation.REMOVE_LIKE_FROM_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePost = state.imagePosts[imagePostId]
            if(imagePost) {
                imagePost.likes = imagePost.likes.filter(l => l.userEmail !== like.userEmail)
            }
        },
        [Mutation.ADD_COMMENT_TO_IMAGE_POST]: (state, { imagePostId, comment}) => {
            const imagePost = state.imagePosts[imagePostId]
            if(imagePost) {
                imagePost.comments.push(comment)
            }
        },
        [Mutation.UPDATE_COMMENT_ON_IMAGE_POST]: (state, { imagePostId, comment}) => {
            const imagePost = state.imagePosts[imagePostId]
            if(imagePost) {
                const commentIndex = imagePost.comments.findIndex(c => c.id === comment.id)
                if(commentIndex !== -1) {
                    imagePost.comments.splice(commentIndex, 1, comment)
                }
            }
        },
        [Mutation.REMOVE_COMMENT_FROM_IMAGE_POST]: (state, { imagePostId, commentId}) => {
            const imagePost = state.imagePosts[imagePostId]
            if(imagePost) {
                imagePost.comments = imagePost.comments.filter(c => c.id !== commentId)
            }
        },
        [Mutation.REMOVE_IMAGE_POST]: (state, imagePostId) => {
            delete state.imagePosts[imagePostId]
        }
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}