<template>
    <div class="text-center text-sm-left">
        <h1 class="text-center">Feed</h1>
        <ul v-if="!isLoading && imagePosts.length > 0">
            <li class="image-post py-5" v-for="post in imagePosts" :key="post.id">
                <image-post-detailed-image class="mt-3"
                    :imagePost="post"
                    :currentUser="currentUser"
                    :canView="true"
                    maxImagePostHeight="50vh"
                    @liked="() => likeImagePost(post)"
                    @unliked="() => unlikeImagePost(post)"/>
                <div class="my-4">
                    <image-post-comment
                        :username="post.username"
                        :content="post.description"
                        :dateCreated="post.dateCreated"
                        :canEdit="false"
                        :canDelete="false"
                        fallbackContent="No description available."
                        @usernameClicked="(username) => viewProfile(username)"/>
                </div>
                <button class="mt-3 w-100" v-b-toggle="'comments-accordion' + post.id">
                    <div>Comments</div>
                </button>
                <b-collapse :id="'comments-accordion' + post.id" accordion>
                    <image-post-comment-list class="mt-3"
                        :comments="post.comments"
                        :canComment="isLoggedIn"
                        :currentUser="currentUser"
                        :imagePostUserEmail="post.userEmail"
                        @created="(content) => createComment(post, content)"
                        @edited="(comment) => editComment(post, comment)"
                        @deleted="(commentId) => deleteComment(post, commentId)"
                        @usernameClicked="viewProfile"/>
                </b-collapse>
            </li>
        </ul>
        <div class="text-center"
            v-else-if="!isLoading">
            <div class="mt-4">Your feed is empty.</div>
            <div class="link mt-3" @click="$router.push({path:'/explore'})">
                Explore posts and follow users who have posted in order to populate your feed.
            </div>
        </div>
        <div class="text-center"
            v-else-if="isLoading">
            <b-spinner class="mt-4 text-center" label="Loading feed..."></b-spinner>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import { ModuleName as AuthenticationModuleName } from '../store/modules/authentication/types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction } from '../store/modules/image-posts/types'
import { ModuleName as FeedModuleName, Action as FeedAction } from '../store/modules/feed/types'

import ImagePostComment from '../components/image-posts/ImagePostComment'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetailedImage from '../components/image-posts/ImagePostDetailedImage'

export default {
    name: "Feed",
    components: {
        ImagePostComment,
        ImagePostCommentList,
        ImagePostDetailedImage
    },
    data: function() {
        return {
            isLoading: false
        }
    },
    computed: {
        ...mapState({
            currentUser: state => state.user.currentUser
        }),
        ...mapGetters(FeedModuleName, ['imagePosts']),
        ...mapGetters(AuthenticationModuleName, ['isLoggedIn'])
    },
    created: async function() {
        this.isLoading = true;
        await this.$store.dispatch(`${FeedModuleName}/${FeedAction.GET_FEED}`)
        this.isLoading = false;
    },
    methods: {
        likeImagePost: function(imagePost) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.LIKE_IMAGE_POST}`, imagePost)
        },
        unlikeImagePost: function(imagePost) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.UNLIKE_IMAGE_POST}`, imagePost)
        },
        createComment: function(imagePost, content) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.CREATE_IMAGE_POST_COMMENT}`, { imagePost, content })
        },
        editComment: function(imagePost, comment) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.UPDATE_IMAGE_POST_COMMENT}`, { imagePost, comment })
        },
        deleteComment: function(imagePost, commentId) {
            this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.DELETE_IMAGE_POST_COMMENT}`, { imagePost, commentId})
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

.image-post:last-child {
    border-bottom: none
}

.link{
    text-decoration: underline;
    cursor: pointer;
}
</style>