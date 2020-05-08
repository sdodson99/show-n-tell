import { Action, Mutation } from './types'

export { ModuleName } from './types'

export default function createUserModule(userService) {
    const state = {
        currentUser: userService.getUser()
    }

    const actions = {
        [Action.ADD_FOLLOWING]({ commit }, following) {
            commit(Mutation.ADD_FOLLOWING_TO_CURRENT_USER, following)
            userService.setUser(state.currentUser)
        },
        [Action.REMOVE_FOLLOWING_BY_USERNAME]({ commit }, username) {
            commit(Mutation.REMOVE_FOLLOWING_BY_USERNAME_FROM_CURRENT_USER, username)
            userService.setUser(state.currentUser)
        }
    }

    const mutations = {
        [Mutation.SET_CURRENT_USER]: (state, currentUser) => state.currentUser = currentUser,
        [Mutation.ADD_FOLLOWING_TO_CURRENT_USER]: (state, following) => state.currentUser.following.push(following),
        [Mutation.REMOVE_FOLLOWING_BY_USERNAME_FROM_CURRENT_USER]: (state, username) => {
            state.currentUser.following = state.currentUser.following.filter(f => f.username !== username)
        }
    }

    return {
        namespaced: true,
        state,
        actions,
        mutations
    }
}