<template>
    <div>
        <image-post-image :class="{ pointer: canView }" 
            :max-height="maxImagePostHeight" 
            :image-uri="imagePost.imageUri"
            @click="viewImagePost"/>
        <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
            <image-post-feedback class="mt-3 mt-sm-1"
                :is-liked="isLiked"
                :can-like="!isUsersImagePost"
                :like-count="imagePost.likes.length"
                :comment-count="imagePost.comments.length"
                @liked="$emit('liked')"
                @unliked="$emit('unliked')"/>
            <more-dropdown v-if="canView || isUsersImagePost">
                <more-dropdown-item
                    v-if="canView"
                    @click="viewImagePost">View</more-dropdown-item>
                <more-dropdown-item
                    v-if="isUsersImagePost" 
                    @click="editImagePost">Edit</more-dropdown-item>
                <more-dropdown-item
                    v-if="isUsersImagePost" 
                    @click="$emit('deleted')">Delete</more-dropdown-item>
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
        viewImagePost: function() {
            if(this.canView) {
                this.$router.push({path: `/explore/${this.imagePost.id}`})
            }
        },
        editImagePost: function() {
            this.$router.push({path: `/image-posts/${this.imagePost.id}/edit`})
        }
    }
}
</script>

<style scoped>

.pointer {
    cursor: pointer;
}

</style>