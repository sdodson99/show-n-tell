<template>
    <div>
        <h1 class="text-center">Search results: {{ query }}</h1>
        <image-post-listing
            v-if="hasImagePosts"
            :image-posts="imagePosts"
            :image-post-service="imagePostService"
            :like-vue-service="likeVueService"
            :current-user="currentUser"/>
        <div class="mt-4 text-center" 
            v-else>
            Your search for <b>{{ query }}</b> did not match any image posts. 
        </div>
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
        currentUser: Object
    },
    data: function() {
        return {
            imagePosts: []
        }
    },
    computed: {
        hasImagePosts: function(){
            return this.imagePosts.length > 0
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
            this.imagePosts = await this.searchService.getImagePosts('sc.dodson4')
        }
    }
}
</script>

<style>

</style>