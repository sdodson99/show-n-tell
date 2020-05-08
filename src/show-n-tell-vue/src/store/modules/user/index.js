import { Action, Mutation } from './types'

export { ModuleName } from './types'

export default function createUserModule(userService) {
    const state = {
        currentUser: userService.getUser()
    }

    const mutations = {
        [Mutation.SET_CURRENT_USER]: (state, currentUser) => state.currentUser = currentUser
    }

    return {
        namespaced: true,
        state,
        mutations
    }
}