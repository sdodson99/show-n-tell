import { Action, Mutation } from './types'

import router from '../../../router'
import ServiceContainer from '../../../services/service-container'

const authenticationService = ServiceContainer.AuthenticationService

const state = {
    currentUser: authenticationService.getUser(),
    isLoggingIn: false
}

const getters = {
    isLoggedIn: (state) => state.currentUser !== null
}

const actions = {
    async [Action.LOGIN]({ commit }, { token, redirectPath, redirectBack }) {
        commit(Mutation.SET_IS_LOGGING_IN, true)

        const user = await authenticationService.login(token)
        commit(Mutation.SET_CURRENT_USER, user)

        if(redirectPath) {
            router.push({path: redirectPath})
        } else if (redirectBack) {
            router.go(-1)
        } else {
            router.push({name: "Home"})
        }

        commit(Mutation.SET_IS_LOGGING_IN, false)
    },
    async [Action.LOGOUT]({ commit }) {
        if(authenticationService.logout()) {
            router.push({name: "Home"})
            commit(Mutation.SET_CURRENT_USER, null)
        }
    }
}

const mutations = {
    [Mutation.SET_CURRENT_USER]: (state, user) => state.currentUser = user,
    [Mutation.SET_IS_LOGGING_IN]: (state, isLoggingIn) => state.isLoggingIn = isLoggingIn
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}