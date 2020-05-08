import { Action, Mutation } from './types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction, Mutation as ImagePostsMutation } from '../image-posts/types'
import { ModuleName as UserModuleName, Action as UserAction } from '../user/types'

import UnauthorizedError from "../../../errors/unauthorized-error";
import NotFoundError from "../../../errors/not-found-error";

import Follower from '../../../models/follower'

export { ModuleName } from './types'

export default function createProfileModule(profileService, followService, router) {
    const state = {
        profile: {},
        imagePostIds: [],
        profileUsername: "",
        profileNotFound: false
    }

    const getters = {
        imagePosts: (state, g, rootState) => state.imagePostIds.map(id => rootState.imagePosts.imagePosts[id]),
        currentUser: (s, g, rootState) => rootState.user.currentUser
    }

    const actions = {
        async [Action.GET_PROFILE_BY_USERNAME]({ commit }, username) {
            commit(Mutation.SET_PROFILE_NOT_FOUND, false)
            commit(Mutation.SET_PROFILE_USERNAME, username)

            try {
                const profile = await profileService.getProfile(state.profileUsername)

                commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, profile.imagePosts, { root: true})
                commit(Mutation.SET_IMAGE_POST_IDS, profile.imagePosts.sort((a, b) => b.dateCreated - a.dateCreated).map(p => p.id))
                commit(Mutation.SET_PROFILE, profile)
            } catch (error) {
                if(error instanceof NotFoundError)
                {
                    commit(Mutation.SET_PROFILE_NOT_FOUND, true)
                }
            }
        },
        async [Action.FOLLOW_PROFILE]({ commit, dispatch, getters }) {
            try {
                const following = await followService.follow(state.profileUsername)

                commit(Mutation.ADD_FOLLOWER_TO_PROFILE, new Follower(getters.currentUser.email, getters.currentUser.username))
                dispatch(`${UserModuleName}/${UserAction.ADD_FOLLOWING}`, following, { root: true })
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                  router.push({path: "/login", query: { back: true }})
                }
            }
        },
        async [Action.UNFOLLOW_PROFILE]({ commit, getters, dispatch }) {
            try {
                const success = await followService.unfollow(state.profileUsername)
                if(success) {
                    commit(Mutation.REMOVE_FOLLOWER_FROM_PROFILE, getters.currentUser.email)
                    dispatch(`${UserModuleName}/${UserAction.REMOVE_FOLLOWING_BY_USERNAME}`, state.profileUsername, { root: true })
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    router.push({path: "/login", query: { back: true }})
                }
            }
        },
        async [Action.DELETE_IMAGE_POST]({ commit, dispatch, rootState }, imagePostId) {
            await dispatch(`${ImagePostsModuleName}/${ImagePostsAction.DELETE_IMAGE_POST}`, imagePostId, { root: true })

            if(!rootState.imagePosts.imagePosts[imagePostId]) {
                commit(Mutation.REMOVE_IMAGE_POST_FROM_PROFILE, imagePostId)
            }
        }
    }

    const mutations = {
        [Mutation.SET_PROFILE]: (state, profile) => state.profile = profile,
        [Mutation.SET_IMAGE_POST_IDS]: (state, imagePostIds) => state.imagePostIds = imagePostIds,
        [Mutation.SET_PROFILE_USERNAME]: (state, profileUsername) => state.profileUsername = profileUsername,
        [Mutation.SET_PROFILE_NOT_FOUND]: (state, profileNotFound) => state.profileNotFound = profileNotFound,
        [Mutation.ADD_FOLLOWER_TO_PROFILE]: (state, follow) => state.profile.followers.push(follow),
        [Mutation.REMOVE_FOLLOWER_FROM_PROFILE]: (state, email) => {
            state.profile.followers = state.profile.followers.filter(f => f.email !== email);
        },
        [Mutation.REMOVE_IMAGE_POST_FROM_PROFILE]: (state, imagePostId) => {
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