<template>
    <div>
        <h1 class="text-center">Feed</h1>
        <ul v-if="isLoaded && imagePosts.length > 0">
            <li class="image-post mx-5 py-5" v-for="post in imagePosts" :key="post.id">
                <image-post-image
                    :imageUri="post.imageUri"
                    maxHeight="50vh"/>
                <div class="mt-3 d-flex flex-column flex-sm-row justify-content-between align-items-center">
                    <image-post-feedback
                        :canLike="!isUsersPost(post)"
                        :liked="isLiked(post)"
                        :likeCount="post.likes.length"
                        :commentCount="post.comments.length"
                        @liked="() => likeImagePost(post)"
                        @unliked="() => unlikeImagePost(post)"/>
                    <image-post-details class="text-center text-sm-right mt-3 mt-sm-0"
                        :username="post.username"
                        :dateCreated="post.dateCreated"/>
                </div>
                <div class="mt-3 text-center">
                    <p>{{ post.description }}</p>
                </div>
                <image-post-comment-list class="mt-5"
                    :comments="post.comments"
                    :canComment="isLoggedIn"
                    @commented="(comment) => createComment(post, comment)"
                    @usernameClicked="viewProfile"/>
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
            <div class="mt-4">Loading your feed...</div>
        </div>
    </div>
</template>

<script>
import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetails from '../components/image-posts/ImagePostDetails'
import UnauthorizedError from '../errors/unauthorized-error'

export default {
    name: "Feed",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        ImagePostCommentList,
        ImagePostDetails
    },
    props: {
        currentUser: Object,
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
        }
    },
    created: async function() {
        try {  
            this.imagePosts = await this.feedService.getFeed()
        } catch (error) {
            if(error instanceof UnauthorizedError) {
                this.$router.push({path: "/login"})
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

.link{
    text-decoration: underline;
    cursor: pointer;
}
</style>