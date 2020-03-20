<template>
    <div>
        <h1 class="text-center">{{ username }}</h1>
        <h3 class="mt-4 text-center text-lg-left">Image Posts</h3>
        <div v-if="hasNoImagePosts" class="mt-4 text-center">
            {{ hasNoImagePostsMessage }}
        </div>
        <ul v-else class="row justify-content-center justify-content-lg-start">
            <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in imagePosts" :key="post.id">
                <image-post-image class="img-post-img" max-height="30vh" @click="() => viewImagePost(post.id)" :imageUri="post.imageUri"/>
                <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
                    <image-post-feedback
                        :canLike="!isUsersProfile"
                        :liked="isLiked(post)"
                        :likeCount="post.likes.length"
                        @liked="() => likeImage(post)"
                        @unliked="() => unlikeImage(post)"/>
                    <more-dropdown>
                        <ul class="my-dropdown">
                            <li @click="() => viewImagePost(post.id)" class="px-3 py-2 my-dropdown-item">View</li>
                            <li v-if="isUsersProfile" @click="() => editImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Edit</li>
                            <li v-if="isUsersProfile" @click="() => deleteImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Delete</li>
                        </ul>
                    </more-dropdown>
                </div>
            </li>
        </ul>
    </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'

import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'
import MoreDropdown from '../components/utilities/MoreDropdown'
import LikeServiceMixin from '../mixins/like-service-mixin'

export default {
    name: "Profile",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown
    },
    mixins: [
        LikeServiceMixin
    ],
    props: {
        imagePostService: Object,
        likeService: Object,
        profileService: Object,
        currentUser: Object
    },
    data: function() {
        return {
            username: "",
            imagePosts: [],
            isLoaded: false
        }
    },
    computed: {
        hasNoImagePosts: function() {
            return this.isLoaded && this.imagePosts.length === 0
        },
        hasNoImagePostsMessage: function() {
            return this.isUsersProfile ? 
                "You have not posted any images yet." :
                "This user has not posted any images yet."
        },
        isUsersProfile: function() {
            return this.currentUser != null && this.currentUser.username === this.username
        },
        isLiked: function() {
            return post => post.likes.some(l => l.userEmail === this.currentUser.email)
        }
    },
    created: function() {
        this.loadImagePosts()
    },
    watch: {
        '$route.params.username': async function() {
            await this.loadImagePosts()
        }
    },
    methods: {
        loadImagePosts: async function() {
            this.username = this.$route.params.username

            if(this.username) {
                const posts = await this.profileService.getImagePosts(this.username)

                this.imagePosts = posts.sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))
                this.isLoaded = true;
            }
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
        },
        likeImage: async function(imagePost) {
            imagePost.likes = await this._likeImage(imagePost)
        },
        unlikeImage: async function(imagePost) {
            imagePost.likes = await this._unlikeImage(imagePost)
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