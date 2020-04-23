import Vuex from 'vuex';
import Vue from 'vue';

import explore from './modules/explore'

Vue.use(Vuex)

export default new Vuex.Store({
    modules: {
        explore
    }
})