<template>
    <div class="text-center text-sm-left">
        <h1 class="text-center">Feed</h1>
        <ul v-if="isLoaded && imagePosts.length > 0">
            <li class="image-post py-5" v-for="post in imagePosts" :key="post.id">
                <image-post-detailed-image class="mt-3"
                    maxImagePostHeight="50vh"
                    :imagePost="post"
                    :imagePostService="imagePostService"
                    :likeVueService="likeVueService"
                    :currentUser="currentUser"
                    @imagePostDeleted="imagePostDeleted"/>
                <div class="my-4">
                    <image-post-comment
                        :content="post.description"
                        fallbackContent="No description available."
                        :username="post.username"
                        :dateCreated="post.dateCreated"
                        @usernameClicked="(username) => viewProfile(username)"/>
                </div>
                <button class="mt-3 w-100" v-b-toggle="'comments-accordion' + post.id">
                    <div>Comments</div>
                </button>
                <b-collapse :id="'comments-accordion' + post.id" accordion>
                    <image-post-comment-list class="mt-3"
                        :comments="post.comments"
                        :canComment="isLoggedIn"
                        @commented="(comment) => createComment(post, comment)"
                        @usernameClicked="viewProfile"/>
                </b-collapse>
            </li>
        </ul>
        <div class="text-center"
            v-else-if="isLoaded">
            <div class="mt-4">Your feed is empty.</div>
            <div class="link mt-3" @click="$router.push({path:'/explore'})">
                Explore posts and follow users who have posted in order to populate your feed.
            </div>
        </div>
        <div class="text-center"
            v-else-if="!isLoaded">
            <b-spinner class="mt-4 text-center" label="Loading feed..."></b-spinner>
        </div>
    </div>
</template>

<script>
import ImagePostComment from '../components/image-posts/ImagePostComment'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetailedImage from '../components/image-posts/ImagePostDetailedImage'
import UnauthorizedError from '../errors/unauthorized-error'

export default {
    name: "Feed",
    components: {
        ImagePostComment,
        ImagePostCommentList,
        ImagePostDetailedImage
    },
    props: {
        userService: Object,
        imagePostService: Object,
        feedService: Object,
        commentVueService: Object,
        likeVueService: Object
    },
    data: function() {
        return {
            imagePosts: [],
            isLoaded: false
        }
    },
    computed: {
        isLoggedIn: function() {
            return this.currentUser != null
        },
        isUsersPost: function() {
            return post => this.isLoggedIn && post.user.email === this.currentUser.email
        },
        isLiked: function() {
            return post => this.isLoggedIn && post.likes.some(l => l.userEmail === this.currentUser.email)
        },
        currentUser: function() {            
            return this.userService.getUser()
        }
    },
    created: async function() {
        try {  
            this.imagePosts = await this.feedService.getFeed()
        } catch (error) {
            if(error instanceof UnauthorizedError) {
                this.$router.push({path: "/login", query: { back: true }})
            }
        }

        this.isLoaded = true;
    },
    methods: {
        likeImagePost: async function(imagePost) {
            imagePost.likes = await this.likeVueService.likeImagePost(imagePost)
        },
        unlikeImagePost: async function(imagePost) {
            imagePost.likes = await this.likeVueService.unlikeImagePost(imagePost)
        },
        createComment: async function(imagePost, comment) {
            imagePost.comments = await this.commentVueService.createComment(imagePost, comment)
        },
        viewProfile: function(username) {
            this.$router.push({path: `/profile/${username}`})
        },
        imagePostDeleted: function(imagePostId) {
            this.imagePosts = this.images.filter(p => p.id !== imagePostId)
        }
    }
}
</script>

<style scoped>
ul {
    list-style: none;
    max-width: 700px;
    margin: auto;
}

.image-post {
    border-bottom: 1px solid var(--color-grayscale-medium);
}

.image-post:last-child {
    border-bottom: none
}

.link{
    text-decoration: underline;
    cursor: pointer;
}
</style>