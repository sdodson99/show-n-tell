<template>
    <div>
        <div v-if="profileNotFound">
            <h1 class="text-center">The profile does not exist.</h1>
            <div class="mt-5 text-center pointer" @click="viewExplore"><u>Go explore instead.</u></div>
        </div>
        <div v-else>
            <h1 class="text-center">{{ profile.username }}</h1>
            <div class="d-flex flex-column align-items-center">
                <div v-if="!isUsersProfile" class="mt-4">
                    <button v-if="!isFollowing" type="submit" @click="followProfile">Follow</button>
                    <button v-else type="submit" @click="unfollowProfile">Unfollow</button>
                </div>
                <div class="mt-4 d-flex flex-column flex-sm-row">
                    <div> {{ followerCount }} followers</div>
                    <div class="mx-3 d-none d-sm-block">|</div>
                    <div class="mt-2 mt-sm-0"> {{ followingCount }} following</div>
                </div>
            </div>
            <h3 class="mt-4 text-center text-lg-left">Image Posts</h3>
            <div v-if="hasNoImagePosts && isUsersProfile" class="mt-4 text-center">
                You have not posted any images yet.
            </div>
            <div v-else-if="hasNoImagePosts" class="mt-4 text-center">
                This user has not posted any images yet.
            </div>
            <ul v-else class="row justify-content-center justify-content-lg-start">
                <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in profile.imagePosts" :key="post.id">
                    <image-post-image class="img-post-img" max-height="30vh" @click="() => viewImagePost(post.id)" :imageUri="post.imageUri"/>
                    <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
                        <image-post-feedback class="mt-2 mt-sm-0"
                            :canLike="!isUsersProfile"
                            :liked="isLiked(post)"
                            :likeCount="post.likes.length"
                            :commentCount="post.comments.length"
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
    </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'

import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'
import MoreDropdown from '../components/utilities/MoreDropdown'

export default {
    name: "Profile",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown
    },
    props: {
        imagePostService: Object,
        likeVueService: Object,
        profileService: Object,
        currentUser: Object
    },
    data: function() {
        return {
            profile: {},
            isLoaded: false,
            profileNotFound: false
        }
    },
    computed: {
        hasNoImagePosts: function() {
            return this.isLoaded && this.profile.imagePosts && this.profile.imagePosts.length === 0
        },
        isUsersProfile: function() {
            return this.currentUser !== null && this.currentUser.username === this.profile.username
        },
        isLiked: function() {
            return post => this.currentUser !== null && post.likes.some(l => l.userEmail === this.currentUser.email)
        },
        followerCount: function() {
            return this.profile.followers && this.profile.followers.length
        },
        followingCount: function() {
            return this.profile.following && this.profile.following.length
        },
        isFollowing: function() {
            return this.currentUser !== null && this.profile.followers && this.profile.followers.some(f => f.followEmail === this.currentUser.email)
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
            const username = this.$route.params.username

            try {
                const profile = await this.profileService.getProfile(username)
                profile.imagePosts = profile.imagePosts.sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))

                this.profile = profile
            } catch (error) {
                this.profileNotFound = true
            }
            
            this.isLoaded = true;
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
                    this.profile.imagePosts = this.profile.imagePosts.filter(p => p.id !== imagePostId)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login"})
                }
            }
        },
        likeImage: async function(imagePost) {
            imagePost.likes = await this.likeVueService.likeImagePost(imagePost)
        },
        unlikeImage: async function(imagePost) {
            imagePost.likes = await this.likeVueService.unlikeImagePost(imagePost)
        },
        followProfile: async function() {

        },
        unfollowProfile: async function() {

        },
        viewExplore: function() {
            this.$router.push({path: "/explore"})
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

.pointer {
    cursor: pointer;
}

</style>