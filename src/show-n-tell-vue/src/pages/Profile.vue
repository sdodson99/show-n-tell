<template>
    <div>
        <div v-if="profileNotFound">
            <h1 class="text-center">The profile does not exist.</h1>
            <div class="mt-5 text-center pointer" @click="viewExplore"><u>Go explore instead.</u></div>
        </div>
        <div v-else>
            <h1 class="text-center">{{ profile.username }}</h1>
            <div class="d-flex flex-column align-items-center">
                <div v-if="isLoaded && !isUsersProfile" class="mt-4">
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
                :current-user="currentUser"/>
        </div>
    </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'
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
        followProfile: async function() {
            try {
                const follow = await this.followService.follow(this.profile.username)
                this.profile.followers.push(follow)
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login"})
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
                    this.$router.push({path: "/login"})
                }
            }
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