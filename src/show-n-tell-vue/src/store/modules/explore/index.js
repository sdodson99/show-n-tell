import { Action, Mutation } from './types'

export default function createExploreModule(imagePostService, randomImagePostService, likeVueService, commentVueService) {
    const state = {
        imagePosts: [],
        currentImagePostIndex: 0,
        noImagePostsAvailable: false,
        imagePostNotFound: false,
        isLoading: false
    }

    const getters = {
        currentImagePost: (state) => state.imagePosts[state.currentImagePostIndex],
        canExplore: (state) => !state.noImagePostsAvailable,
        isShowingLastImage: (state) => state.currentImagePostIndex + 1 === state.imagePosts.length,
        hasPreviousImagePost: (state) => state.currentImagePostIndex > 0,
        currentImagePostRoute: (s, getters) => `/explore/${getters.currentImagePost.id}`
    }

    const actions = {
        async [Action.FETCH_RANDOM_IMAGE_POST]({ commit }) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const randomImagePost = await randomImagePostService.getRandom();

                const existingImagePost = state.imagePosts.find(p => p.id === randomImagePost.id)
        
                commit(Mutation.ADD_IMAGE_POST, existingImagePost || randomImagePost)
            } catch (error) {
                commit(Mutation.SET_HAS_NO_IMAGE_POSTS, true)
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.FETCH_IMAGE_POST_BY_ID]({ commit }, id) {
            commit(Mutation.SET_IS_LOADING, true)

            try {
                const imagePost = await imagePostService.getById(id);
                
                commit(Mutation.ADD_IMAGE_POST, imagePost)
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
        } ,
        [Action.PREVIOUS_IMAGE_POST]({ commit, getters }) {
            if(getters.hasPreviousImagePost) {
                commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.currentImagePostIndex - 1)
            }
        },
        async [Action.LIKE_IMAGE_POST]({ commit, getters }) {
            if(getters.currentImagePost) {
                const newLike = await likeVueService.likeImagePost(getters.currentImagePost)
                if(newLike) {
                    commit(Mutation.ADD_LIKE_TO_CURRENT_IMAGE_POST, newLike)
                }
            }
        },
        async [Action.UNLIKE_IMAGE_POST]({ commit, getters}) {
            if(getters.currentImagePost) {
                const removedLike = await likeVueService.unlikeImagePost(getters.currentImagePost)
                if(removedLike) {
                    commit(Mutation.REMOVE_LIKE_FROM_CURRENT_IMAGE_POST, removedLike);
                }
            }
        },
        async [Action.CREATE_IMAGE_POST_COMMENT]({ commit, getters }, comment) {
            if(getters.currentImagePost) {
                const createdComment = await commentVueService.createComment(getters.currentImagePost, comment, getters.currentImagePostRoute)
                if(createdComment) {
                    commit(Mutation.ADD_COMMENT_TO_CURRENT_IMAGE_POST, createdComment)
                }
            }
        },
        async [Action.UPDATE_IMAGE_POST_COMMENT]({ commit, getters }, comment) {
            if(getters.currentImagePost) {
                const updatedComment = await commentVueService.updateComment(getters.currentImagePost, comment.id, comment.content, getters.currentImagePostRoute)
                if(updatedComment) {
                    commit(Mutation.UPDATE_COMMENT_ON_CURRENT_IMAGE_POST, updatedComment)
                }
            }
        },
        async [Action.DELETE_IMAGE_POST_COMMENT]({ commit, getters }, commentId) {
            if(getters.currentImagePost) {
                const deletedComment = await commentVueService.deleteComment(getters.currentImagePost, commentId, getters.currentImagePostRoute)
                if(deletedComment) {
                    commit(Mutation.REMOVE_COMMENT_FROM_CURRENT_IMAGE_POST, commentId)
                }
            }        
        },
        async [Action.DELETE_IMAGE_POST]({ commit, getters, dispatch }) {
            if(getters.currentImagePost) {
                if(await imagePostService.delete(getters.currentImagePost.id)) {
                    commit(Mutation.REMOVE_IMAGE_POST, getters.currentImagePost.id)

                    // Get an initial image if length is 0.
                    if(state.imagePosts.length === 0) {
                        await dispatch(Action.FETCH_RANDOM_IMAGE_POST)
                    }

                    // Coerce current image index to the last image available if index larger than images length.
                    if(state.currentImagePostIndex >= state.imagePosts.length) {
                        commit(Mutation.SET_CURRENT_IMAGE_POST_INDEX, state.imagePosts.length - 1)
                    }
                }
            }
        }
    }

    const mutations = {
        [Mutation.ADD_IMAGE_POST]: (state, imagePost) => state.imagePosts.push(imagePost),
        [Mutation.REMOVE_IMAGE_POST]: (state, id) => state.imagePosts = state.imagePosts.filter(p => p.id !== id),
        [Mutation.ADD_LIKE_TO_CURRENT_IMAGE_POST]: (state, like) => state.imagePosts[state.currentImagePostIndex].likes.push(like),
        [Mutation.REMOVE_LIKE_FROM_CURRENT_IMAGE_POST]: (state, like) => {
            state.imagePosts[state.currentImagePostIndex].likes = state.imagePosts[state.currentImagePostIndex].likes.filter(l => l.userEmail !== like.userEmail)
        },
        [Mutation.ADD_COMMENT_TO_CURRENT_IMAGE_POST]: (state, comment) => state.imagePosts[state.currentImagePostIndex].comments.push(comment),
        [Mutation.UPDATE_COMMENT_ON_CURRENT_IMAGE_POST]: (state, comment) => {
            const currentImagePost = state.imagePosts[state.currentImagePostIndex]
            const commentIndex = currentImagePost.comments.findIndex(c => c.id === comment.id)
            if(commentIndex !== -1) {
                currentImagePost.comments.splice(commentIndex, 1, comment)
            }
        },
        [Mutation.REMOVE_COMMENT_FROM_CURRENT_IMAGE_POST]: (state, commentId) => {
            state.imagePosts[state.currentImagePostIndex].comments = state.imagePosts[state.currentImagePostIndex].comments.filter(c => c.id !== commentId)
        },
        [Mutation.SET_CURRENT_IMAGE_POST_INDEX]: (state, currentImagePostIndex) => state.currentImagePostIndex = currentImagePostIndex,
        [Mutation.SET_NO_IMAGE_POSTS_AVAILABLE]: (state, noImagePostsAvailable) => state.noImagePostsAvailable = noImagePostsAvailable,
        [Mutation.SET_IMAGE_POST_NOT_FOUND]: (state, imagePostNotFound) => state.imagePostNotFound = imagePostNotFound,
        [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading,
    }

    return {
        namespaced: true,
        state,
        getters,
        actions,
        mutations
    }
}