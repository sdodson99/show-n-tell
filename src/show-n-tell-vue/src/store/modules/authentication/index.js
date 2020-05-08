import { Action } from './types'
import { ModuleName as UserModuleName, Mutation as UserMutation } from '../user/types'

export { ModuleName } from './types'

export default function createAuthenticationModule(authenticationService, router) {
    const getters = {
        isLoggedIn: (s, g, rootState) => rootState.user.currentUser !== null
    }

    const actions = {
        async [Action.LOGIN]({ commit }, { token, redirectPath, redirectBack }) {
            const user = await authenticationService.login(token)
            commit(`${UserModuleName}/${UserMutation.SET_CURRENT_USER}`, user, { root: true })

            if(redirectPath) {
                router.push({path: redirectPath})
            } else if (redirectBack) {
                router.go(-1)
            } else {
                router.push({name: "Home"})
            }
        },
        async [Action.LOGOUT]({ commit }) {
            if(authenticationService.logout()) {
                router.push({name: "Home"})
                commit(`${UserModuleName}/${UserMutation.SET_CURRENT_USER}`, null, { root: true })
            }
        }
    }

    return {
        namespaced: true,
        getters,
        actions
    }
}