<template>
    <div>
        <div v-if="!profileNotFound">
            <h1 class="text-center">{{ profileUsername }}</h1>
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
                    :current-user="currentUser"
                    @liked="likeImagePost"
                    @unliked="unlikeImagePost"
                    @deleted="deleteImagePost"/>
            </div>
            <div v-else class="text-center">
                <b-spinner class="mt-4" label="Loading profile..."></b-spinner>
            </div>
        </div>
        <div v-else>
            <h1 class="text-center">The profile '{{ profileUsername }}' does not exist.</h1>
            <div class="mt-3 text-center pointer" @click="viewExplore"><u>Go explore instead.</u></div>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";
import { ModuleName as AuthenticationModuleName } from "../store/modules/authentication/types"
import { ModuleName as ProfileModuleName, Action } from "../store/modules/profile/types"

import ImagePostListing from '../components/image-posts/ImagePostListing'

export default {
    name: "Profile",
    components: {
        ImagePostListing
    },
    computed: {
        ...mapState({
            profile: state => state.profile.profile,
            profileUsername: state => state.profile.profileUsername,
            profileNotFound: state => state.profile.profileNotFound,
            isLoading: state => state.profile.isLoading,
            currentUser: state => state.authentication.currentUser
        }),
        hasNoImagePosts: function() {
            return this.profile.imagePosts && this.profile.imagePosts.length === 0
        },
        isUsersProfile: function() {
            return this.currentUser && this.currentUser.username === this.profileUsername
        },
        followerCount: function() {
            return this.profile.followers && this.profile.followers.length
        },
        followingCount: function() {
            return this.profile.following && this.profile.following.length
        },
        isFollowing: function() {
            return this.currentUser && this.profile.followers && this.profile.followers.some(f => f.followerEmail === this.currentUser.email)
        }
    },
    created: function() {
        this.loadProfile()
    },
    watch: {
        '$route.params.username': function() {
            this.loadProfile()
        }
    },
    methods: {
        loadProfile: function() {
            this.$store.dispatch(`${ProfileModuleName}/${Action.GET_PROFILE_BY_USERNAME}`, this.$route.params.username)
        },
        followProfile: function() {
            this.$store.dispatch(`${ProfileModuleName}/${Action.FOLLOW_PROFILE}`)
        },
        unfollowProfile: function() {
            this.$store.dispatch(`${ProfileModuleName}/${Action.UNFOLLOW_PROFILE}`)
        },
        likeImagePost: function(imagePost) {
            this.$store.dispatch(`${ProfileModuleName}/${Action.LIKE_IMAGE_POST}`, imagePost)
        },
        unlikeImagePost: function(imagePost) {
            this.$store.dispatch(`${ProfileModuleName}/${Action.UNLIKE_IMAGE_POST}`, imagePost)
        },
        deleteImagePost: function(imagePost) {
            this.$store.dispatch(`${ProfileModuleName}/${Action.DELETE_IMAGE_POST}`, imagePost.id)
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