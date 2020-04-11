<template>
    <div>
        <h1 class="text-center">Search results: {{ query }}</h1>
        <div class="mt-4 text-center" 
            v-if="isLoading">
            Searching for <b>{{ query }}</b>...
        </div>
        <div class="mt-4 text-center" 
            v-else-if="!hasImagePosts">
            Your search for <b>{{ query }}</b> did not match any image posts. 
        </div>
        <image-post-listing
            v-else-if="hasImagePosts && !isLoading"
            :image-posts="imagePosts"
            :image-post-service="imagePostService"
            :like-vue-service="likeVueService"
            :current-user="currentUser"
            @imagePostDeleted="imagePostDeleted"/>
    </div>
</template>

<script>
import ImagePostListing from '../components/image-posts/ImagePostListing'

export default {
    name: "Search",
    components: {
        ImagePostListing
    },
    props: {
        query: String,
        searchService: Object,
        imagePostService: Object,
        likeVueService: Object,
        userService: Object
    },
    data: function() {
        return {
            imagePosts: [],
            isLoading: true
        }
    },
    computed: {
        hasImagePosts: function(){
            return this.imagePosts.length > 0
        },
        currentUser: function() {
            return this.userService.getUser()
        }
    },
    created: async function() {
        await this.searchImagePosts()
    },
    watch: {
        query: async function() {
            await this.searchImagePosts()
        }
    },
    methods: {
        searchImagePosts: async function() {
            this.isLoading = true
            this.imagePosts = await this.searchService.searchImagePosts(this.query)
            this.isLoading = false
        },
        imagePostDeleted: function(imagePostId) {
            this.imagePosts = this.imagePosts.filter(p => p.id !== imagePostId)
        }
    }
}
</script>

<style>

</style>