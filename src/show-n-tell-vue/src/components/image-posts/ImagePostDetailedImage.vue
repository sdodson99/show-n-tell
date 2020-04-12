<template>
    <div>
        <image-post-image :class="{ pointer: canView }" 
            :max-height="maxImagePostHeight" 
            :imageUri="imagePost.imageUri"
            @click="viewImagePost"/>
        <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
            <image-post-feedback class="mt-3 mt-sm-1"
                :canLike="!isUsersImagePost"
                :liked="isLiked"
                :likeCount="imagePost.likes.length"
                :commentCount="imagePost.comments.length"
                @liked="likeImage"
                @unliked="unlikeImage"/>
            <more-dropdown v-if="canView || isUsersImagePost">
                <ul class="my-dropdown">
                    <li class="px-3 py-2 my-dropdown-item" 
                        v-if="canView"
                        @click="viewImagePost">View</li>
                    <li class="px-3 py-2 my-dropdown-item"
                        v-if="isUsersImagePost" 
                        @click="editImagePost">Edit</li>
                    <li class="px-3 py-2 my-dropdown-item" 
                        v-if="isUsersImagePost" 
                        @click="deleteImagePost">Delete</li>
                </ul>
            </more-dropdown>
        </div>
    </div>
</template>

<script>
import UnauthorizedError from '../../errors/unauthorized-error'

import ImagePostImage from './ImagePostImage'
import ImagePostFeedback from './ImagePostFeedback'
import MoreDropdown from '../utilities/MoreDropdown'

export default {
    name: "ImagePostDetailedImage",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown
    },
    props: {
        imagePost: Object,
        imagePostService: Object,
        likeVueService: Object,
        currentUser: Object,
        maxImagePostHeight: String,
        canView: Boolean
    },
    computed: {
        isUsersImagePost: function() {
            return this.currentUser !== null && this.currentUser.username === this.imagePost.username
        },
        isLiked: function() {
            return this.currentUser !== null && this.imagePost.likes.some(l => l.userEmail === this.currentUser.email)
        }
    },
    methods: {
        likeImage: async function() {
            this.imagePost.likes = await this.likeVueService.likeImagePost(this.imagePost)
        },
        unlikeImage: async function() {
            this.imagePost.likes = await this.likeVueService.unlikeImagePost(this.imagePost)
        },
        viewImagePost: function() {
            this.$router.push({path: `/explore/${this.imagePost.id}`})
        },
        editImagePost: function() {
            this.$router.push({path: `/imagePosts/${this.imagePost.id}/edit`})
        },
        deleteImagePost: async function() {
            const imagePostId = this.imagePost.id

            try {
                const success = await this.imagePostService.delete(imagePostId)

                if(success) {
                    this.$emit("imagePostDeleted", imagePostId)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login", query: { back: true }})
                }
            }
        }
    }
}
</script>

<style scoped>
ul{
    list-style: none;
}

.pointer {
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