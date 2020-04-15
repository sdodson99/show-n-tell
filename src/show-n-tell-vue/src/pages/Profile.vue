<template>
    <div>
        <div v-if="profileFound">
            <h1 class="text-center">{{ username }}</h1>
            <div v-if="!isLoading">
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
                <image-post-listing 
                    :image-posts="profile.imagePosts" 
                    :like-vue-service="likeVueService"
                    :image-post-service="imagePostService"
                    :current-user="currentUser"
                    @imagePostDeleted="imagePostDeleted"/>
            </div>
            <div v-else class="text-center">
                <b-spinner class="mt-4" label="Loading profile..."></b-spinner>
            </div>
        </div>
        <div v-else>
            <h1 class="text-center">The profile '{{ username }}' does not exist.</h1>
            <div class="mt-3 text-center pointer" @click="viewExplore"><u>Go explore instead.</u></div>
        </div>
    </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'
import NotFoundError from '../errors/not-found-error'
import ImagePostListing from '../components/image-posts/ImagePostListing'

export default {
    name: "Profile",
    components: {
        ImagePostListing
    },
    props: {
        imagePostService: Object,
        likeVueService: Object,
        profileService: Object,
        followService: Object,
        userService: Object
    },
    data: function() {
        return {
            username: "",
            profile: {},
            isLoading: true,
            profileFound: true
        }
    },
    computed: {
        hasNoImagePosts: function() {
            return this.profile.imagePosts && this.profile.imagePosts.length === 0
        },
        isUsersProfile: function() {
            return this.currentUser !== null && this.currentUser.username === this.profile.username
        },
        followerCount: function() {
            return this.profile.followers && this.profile.followers.length
        },
        followingCount: function() {
            return this.profile.following && this.profile.following.length
        },
        isFollowing: function() {
            return this.currentUser !== null && this.profile.followers && this.profile.followers.some(f => f.followerEmail === this.currentUser.email)
        },
        currentUser: function() {
            return this.userService.getUser()
        }
    },
    created: function() {
        this.loadProfile()
    },
    watch: {
        '$route.params.username': async function() {
            await this.loadProfile()
        }
    },
    methods: {
        loadProfile: async function() {
            this.isLoading = true
            this.profileFound = true

            this.username = this.$route.params.username

            try {
                const profile = await this.profileService.getProfile(this.username)
                profile.imagePosts = profile.imagePosts.sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))

                this.profile = profile
            } catch (error) {
                if(error instanceof NotFoundError)
                {
                    this.profileFound = false
                }
            }
            
            this.isLoading = false;
        },
        followProfile: async function() {
            try {
                const follow = await this.followService.follow(this.profile.username)
                this.profile.followers.push(follow)
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                  this.$router.push({path: "/login", query: { back: true }})
                }
            }
        },
        unfollowProfile: async function() {
            try {
                const success = await this.followService.unfollow(this.profile.username)

                if(success) {
                    this.profile.followers = this.profile.followers.filter(f => f.followerEmail !== this.currentUser.email)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login", query: { back: true }})
                }
            }
        },
        imagePostDeleted: function(imagePostId) {
            this.profile.imagePosts = this.profile.imagePosts.filter(p => p.id !== imagePostId)
        },
        viewExplore: function() {
            this.$router.push({path: "/explore"})
        }
    }
}
</script>

<style scoped>

.pointer {
    cursor: pointer;
}

</style>