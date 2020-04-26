<template>
    <div>
        <h1 class="text-center">Search results: {{ query }}</h1>
        <div class="text-center" 
            v-if="isLoading">
            <b-spinner class="mt-4 text-center" label="Searching..."></b-spinner>
        </div>
        <div class="mt-4 text-center" 
            v-else-if="!hasImagePosts">
            Your search for <b>{{ query }}</b> did not match any image posts. 
        </div>
        <image-post-listing
            v-else-if="hasImagePosts && !isLoading"
            :image-posts="imagePosts"
            :current-user="currentUser"
            @liked="likeImagePost"
            @unliked="unlikeImagePost"
            @deleted="deleteImagePost"/>
    </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";
import { ModuleName, Action } from "../store/modules/search/types"

import ImagePostListing from '../components/image-posts/ImagePostListing'

export default {
    name: "Search",
    components: {
        ImagePostListing
    },
    props: {
        query: String
    },
    computed: {
        ...mapState({
            imagePosts: (state) => state.search.imagePosts,
            isLoading: (state) => state.search.isLoading,
            currentUser: (state) => state.authentication.currentUser
        }),
        hasImagePosts: function(){
            return this.imagePosts.length > 0
        }
    },
    created: function() {
        this.searchImagePosts()
    },
    watch: {
        query: function() {
            this.searchImagePosts()
        }
    },
    methods: {
        searchImagePosts: function() {
            this.$store.dispatch(`${ModuleName}/${Action.SEARCH_IMAGE_POSTS}`, this.query)
        },
        likeImagePost: function(imagePost) {
            this.$store.dispatch(`${ModuleName}/${Action.LIKE_IMAGE_POST}`, imagePost)
        },
        unlikeImagePost: function(imagePost) {
            this.$store.dispatch(`${ModuleName}/${Action.UNLIKE_IMAGE_POST}`, imagePost)
        },
        deleteImagePost: function(imagePost) {
            this.$store.dispatch(`${ModuleName}/${Action.DELETE_IMAGE_POST}`, imagePost.id)
        }
    }
}
</script>