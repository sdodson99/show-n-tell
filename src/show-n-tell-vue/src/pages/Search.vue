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
import { ModuleName as SearchModuleName, Action as SearchAction } from "../store/modules/search/types"
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction } from "../store/modules/image-posts/types"

import ImagePostListing from '../components/image-posts/ImagePostListing'

export default {
    name: "Search",
    components: {
        ImagePostListing
    },
    props: {
        query: String
    },
    data: function() {
        return {
            isLoading: false
        }
    },
    computed: {
        ...mapState({
            currentUser: (state) => state.user.currentUser
        }),
        ...mapGetters(SearchModuleName, ['imagePosts']),
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
        searchImagePosts: async function() {
            this.isLoading = true;
            await this.$store.dispatch(`${SearchModuleName}/${SearchAction.SEARCH_IMAGE_POSTS}`, this.query)
            this.isLoading = false;
        },
        likeImagePost: function(imagePost) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.LIKE_IMAGE_POST}`, imagePost)
        },
        unlikeImagePost: function(imagePost) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.UNLIKE_IMAGE_POST}`, imagePost)
        },
        deleteImagePost: function(imagePost) {
            this.$store.dispatch(`${SearchModuleName}/${SearchAction.DELETE_IMAGE_POST}`, imagePost.id)
        }
    }
}
</script>