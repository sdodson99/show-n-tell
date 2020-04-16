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
                <more-dropdown-item
                    v-if="canView"
                    @click="viewImagePost">View</more-dropdown-item>
                <more-dropdown-item
                    v-if="isUsersImagePost" 
                    @click="editImagePost">Edit</more-dropdown-item>
                <more-dropdown-item
                    v-if="isUsersImagePost" 
                    @click="deleteImagePost">{{ isDeleting ? "Deleting..." : "Delete"}}</more-dropdown-item>
            </more-dropdown>
        </div>
    </div>
</template>

<script>
import UnauthorizedError from '../../errors/unauthorized-error'

import ImagePostImage from './ImagePostImage'
import ImagePostFeedback from './ImagePostFeedback'
import MoreDropdown from '../utilities/MoreDropdown'
import MoreDropdownItem from '../utilities/MoreDropdownItem'

export default {
    name: "ImagePostDetailedImage",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown,
        MoreDropdownItem
    },
    props: {
        imagePost: Object,
        imagePostService: Object,
        likeVueService: Object,
        currentUser: Object,
        maxImagePostHeight: String,
        canView: Boolean
    },
    data: function() {
        return {
            isDeleting: false
        }
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
            if(this.canView) {
                this.$router.push({path: `/explore/${this.imagePost.id}`})
            }
        },
        editImagePost: function() {
            this.$router.push({path: `/imagePosts/${this.imagePost.id}/edit`})
        },
        deleteImagePost: async function() {
            this.isDeleting = true

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

            this.isDeleting = false
        }
    }
}
</script>

<style scoped>

.pointer {
    cursor: pointer;
}

</style>