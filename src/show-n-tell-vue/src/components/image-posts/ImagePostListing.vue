<template>
    <ul class="row justify-content-center justify-content-lg-start">
        <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in imagePosts" :key="post.id">
            <image-post-detailed-image class="image d-flex flex-column flex-grow-1"
                maxImagePostHeight="30vh"
                :imagePost="post"
                :currentUser="currentUser"
                :canView="true"
                @image-post-deleted="() => imagePostDeleted()"/>
        </li>
    </ul>
</template>

<script>
import ImagePostDetailedImage from './ImagePostDetailedImage'

export default {
    name: "ImagePostListing",
    components: {
        ImagePostDetailedImage
    },
    props: {
        imagePosts: Array,
        currentUser: Object
    }, 
    methods: {
        imagePostDeleted: function() {
            console.log('test');
            
            //this.$emit('imagePostDeleted', id);
        }
    }
}
</script>

<style scoped>
.image {
    height: 100%;
}

ul {
    list-style: none;
}
</style>