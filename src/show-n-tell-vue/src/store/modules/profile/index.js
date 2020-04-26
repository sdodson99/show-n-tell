import { Action, Mutation } from './types'

import UnauthorizedError from "../../../errors/unauthorized-error";
import NotFoundError from "../../../errors/not-found-error";

export default function createProfileModule(profileService, followService, imagePostService, likeVueService, router) {
    const state = {
        profile: {},
        profileUsername: "",
        profileNotFound: false,
        isLoading: false
    }

    const actions = {
        async [Action.GET_PROFILE_BY_USERNAME]({ commit }, username) {
            commit(Mutation.SET_IS_LOADING, true)
            commit(Mutation.SET_PROFILE_NOT_FOUND, false)
            commit(Mutation.SET_PROFILE_USERNAME, username)

            try {
                const profile = await profileService.getProfile(state.profileUsername)
                profile.imagePosts = profile.imagePosts.sort((a, b) => b.dateCreated - a.dateCreated)

                commit(Mutation.SET_PROFILE, profile)
            } catch (error) {
                if(error instanceof NotFoundError)
                {
                    commit(Mutation.SET_PROFILE_NOT_FOUND, true)
                }
            }

            commit(Mutation.SET_IS_LOADING, false)
        },
        async [Action.FOLLOW_PROFILE]({ commit }) {
            try {
                const follow = await followService.follow(state.profileUsername)
                commit(Mutation.ADD_FOLLOW_TO_PROFILE, follow)
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                  router.push({path: "/login", query: { back: true }})
                }
            }
        },
        async [Action.UNFOLLOW_PROFILE]({ commit, rootState }) {
            try {
                const success = await followService.unfollow(state.profileUsername)
                if(success) {
                    commit(Mutation.REMOVE_FOLLOW_FROM_PROFILE, rootState.authentication.currentUser.email)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }
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
                commit(Mutation.REMOVE_IMAGE_POST_FROM_PROFILE, imagePostId)
            }
        }
    }

    const mutations = {
        [Mutation.SET_PROFILE]: (state, profile) => state.profile = profile,
        [Mutation.SET_PROFILE_USERNAME]: (state, profileUsername) => state.profileUsername = profileUsername,
        [Mutation.SET_IS_LOADING]: (state, isLoading) => state.isLoading = isLoading,
        [Mutation.SET_PROFILE_NOT_FOUND]: (state, profileNotFound) => state.profileNotFound = profileNotFound,
        [Mutation.ADD_FOLLOW_TO_PROFILE]: (state, follow) => state.profile.followers.push(follow),
        [Mutation.REMOVE_FOLLOW_FROM_PROFILE]: (state, email) => {
            state.profile.followers = state.profile.followers.filter(f => f.followerEmail !== email);
        },
        [Mutation.ADD_LIKE_TO_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePostIndex = state.profile.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.profile.imagePosts[imagePostIndex].likes.push(like)
            }
        },
        [Mutation.REMOVE_LIKE_FROM_IMAGE_POST]: (state, { imagePostId, like }) => {
            const imagePostIndex = state.profile.imagePosts.findIndex((p) => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.profile.imagePosts[imagePostIndex].likes = state.profile.imagePosts[imagePostIndex].likes.filter(l => l.userEmail !== like.userEmail)
            }
        },
        [Mutation.REMOVE_IMAGE_POST_FROM_PROFILE]: (state, imagePostId) => {
            const imagePostIndex = state.profile.imagePosts.findIndex(p => p.id === imagePostId)
            if(imagePostIndex !== -1) {
                state.profile.imagePosts = state.profile.imagePosts.filter(p => p.id !== imagePostId)
            }
        }
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}