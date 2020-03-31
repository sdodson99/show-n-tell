<template>
    <ul class="row justify-content-center justify-content-lg-start">
        <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in imagePosts" :key="post.id">
            <image-post-image class="img-post-img" max-height="30vh" @click="() => viewImagePost(post.id)" :imageUri="post.imageUri"/>
            <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
                <image-post-feedback class="mt-2 mt-sm-0"
                    :canLike="!isUsersImagePost(post)"
                    :liked="isLiked(post)"
                    :likeCount="post.likes.length"
                    :commentCount="post.comments.length"
                    @liked="() => likeImage(post)"
                    @unliked="() => unlikeImage(post)"/>
                <more-dropdown>
                    <ul class="my-dropdown">
                        <li @click="() => viewImagePost(post.id)" class="px-3 py-2 my-dropdown-item">View</li>
                        <li v-if="isUsersImagePost(post)" @click="() => editImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Edit</li>
                        <li v-if="isUsersImagePost(post)" @click="() => deleteImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Delete</li>
                    </ul>
                </more-dropdown>
            </div>
        </li>
    </ul>
</template>

<script>
import UnauthorizedError from '../../errors/unauthorized-error'

import ImagePostImage from './ImagePostImage'
import ImagePostFeedback from './ImagePostFeedback'
import MoreDropdown from '../utilities/MoreDropdown'

export default {
    name: "ImagePostListing",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown
    },
    props: {
        imagePosts: Array,
        imagePostService: Object,
        likeVueService: Object,
        currentUser: Object
    },
    computed: {
        isUsersImagePost: function() {
            return post => this.currentUser !== null && this.currentUser.username === post.username
        },
        isLiked: function() {
            return post => this.currentUser !== null && post.likes.some(l => l.userEmail === this.currentUser.email)
        }
    },
    methods: {
        likeImage: async function(imagePost) {
            imagePost.likes = await this.likeVueService.likeImagePost(imagePost)
        },
        unlikeImage: async function(imagePost) {
            imagePost.likes = await this.likeVueService.unlikeImagePost(imagePost)
        },
        viewImagePost: function(imagePostId) {
            this.$router.push({path: `/explore/${imagePostId}`})
        },
        editImagePost: function(imagePostId) {
            this.$router.push({path: `/imagePosts/${imagePostId}/edit`})
        },
        deleteImagePost: async function(imagePostId) {
            try {
                const success = await this.imagePostService.delete(imagePostId)

                if(success) {
                    this.imagePosts = this.imagePosts.filter(p => p.id !== imagePostId)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login"})
                }
            }
        }
    }
}
</script>

<style scoped>
ul {
    list-style: none;
}

.img-post-img {
    cursor: pointer;
}

.my-dropdown{
    border: 1px solid var(--color-primary-dark);
    border-radius: 3px;
    background: white;
}

.my-dropdown-item{
    white-space: nowrap;
    cursor: pointer;
}

.my-dropdown-item:hover {
    background: var(--color-grayscale-light);
}
</style>