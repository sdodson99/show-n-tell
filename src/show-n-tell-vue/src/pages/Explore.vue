<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" 
        :disabled="!canExplore" 
        @click="onNextClick">Next</button>
      <button class="m-1 order-sm-1" type="button" 
        :disabled="!hasPreviousImagePost" 
        @click="onPreviousClick">Previous</button>
    </div>
    <div id="image-post" class="p-1" 
      v-if="!isLoading && currentImagePost && currentImagePost.imageUri">
      <image-post-detailed-image class="mt-3"
          maxImagePostHeight="50vh"
          :imagePost="currentImagePost"
          :imagePostService="imagePostService"
          :likeVueService="likeVueService"
          :currentUser="currentUser"
          :canView="false"
          @imagePostDeleted="imagePostDeleted"/>
      <div class="my-4">
        <image-post-comment class="text-center text-sm-left"
          :content="currentImagePost.description"
          :canEdit="false"
          :canDelete="false"
          fallbackContent="No description available."
          :username="currentImagePost.username"
          :dateCreated="currentImagePost.dateCreated"
          @usernameClicked="(username) => onProfileClick(username)"/>
      </div>
      <div class="my-4 text-center text-sm-left">
        <h3>Comments</h3>
        <image-post-comment-list class="mt-3" 
          :comments="currentImagePost.comments"
          :currentUser="currentUser"
          :imagePostUserEmail="currentImagePost.userEmail"
          :can-comment="isLoggedIn"
          @commented="createComment"
          @usernameClicked="onProfileClick"
          @edited="editComment"
          @deleted="deleteComment"/>
      </div>
    </div>
    <div class="mt-3"
      v-else-if="!isLoading">
      <h3 class="text-center">{{ noImageMessage }}</h3>
    </div>
    <div class="text-center"
        v-else>
        <b-spinner class="mt-4 text-center" label="Loading..."></b-spinner>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import { Action } from '../store/modules/explore/types'

import UnauthorizedError from '../errors/unauthorized-error'
import NotFoundError from '../errors/not-found-error'

import ImagePostComment from '../components/image-posts/ImagePostComment'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetailedImage from '../components/image-posts/ImagePostDetailedImage'

export default {
  name: "Explore",
  props: {
    imagePostService: Object,
    likeVueService: Object,
    commentVueService: Object,
    commentService: Object,
    userService: Object
  },
  components: {
    ImagePostComment,
    ImagePostCommentList,
    ImagePostDetailedImage
  },
  computed: {
    ...mapState({
      isLoading: (state) => state.explore.isLoading,
      noImagePostsAvailable: (state) => state.explore.noImagePostsAvailable,
      imagePostNotFound: (state) => state.explore.imagePostNotFound
    }),
    ...mapGetters('explore', ['currentImagePost', 'canExplore', 'hasPreviousImagePost']),
    noImageMessage: function() {
      if(this.noImagePostsAvailable) return "No images have been posted."
      if(this.imagePostNotFound) return "The selected image does not exist."
      return ""
    },
    isLiked: function() {
      return this.isLoggedIn && this.currentImage.likes.some(l => l.userEmail === this.currentUser.email)
    },
    isUsersPost: function() {
      return this.isLoggedIn && this.currentImage.userEmail === this.currentUser.email
    },
    isLoggedIn: function() {
      return this.currentUser !== null
    },
    currentUser: function() {
      return this.userService.getUser()
    },
    currentImagePostRoute: function() {
      return `/explore/${this.currentImage.id}`
    }
  },
  created: async function() {
    const initialImageId = this.$route.params.initialId;
    
    if(initialImageId) {
      this.$store.dispatch(`explore/${Action.FETCH_IMAGE_POST_BY_ID}`, initialImageId)
    } else {
      this.$store.dispatch(`explore/${Action.FETCH_RANDOM_IMAGE_POST}`)
    }
  },
  methods: {
    onNextClick: async function() {
      this.$store.dispatch(`explore/${Action.NEXT_IMAGE_POST}`)
    },
    onPreviousClick: function() {
      this.$store.dispatch(`explore/${Action.PREVIOUS_IMAGE_POST}`)
    },
    createComment: async function(comment) {
      this.currentImage.comments = await this.commentVueService.createComment(this.currentImage, comment, this.currentImagePostRoute)
    },
    editComment: async function(comment) {
      this.currentImage.comments = await this.commentVueService.updateComment(this.currentImage, comment.id, comment.content, this.currentImagePostRoute)
    },
    deleteComment: async function(commentId) {
      this.currentImage.comments = await this.commentVueService.deleteComment(this.currentImage, commentId, this.currentImagePostRoute)
    },
    imagePostDeleted: async function(id) {
      this.images = this.images.filter(p => p.id !== id)

      // Get an initial image if length is 0.
      if(this.images.length === 0) {
        await this.getRandomImage()
      }

      // Coerce current image index to the last image available if index larger than images length.
      if(this.currentImageIndex >= this.images.length) {
        this.currentImageIndex = this.images.length - 1
      }
    },
    onProfileClick: function(username) {
      this.$router.push({path: `/profile/${username}`})
    }
  }
};
</script>

<style scoped>
  #image-post {
    max-width: 700px;
    margin: auto;
  }

  .image-post-description {
    border: 1px solid var(--color-primary-dark);
    border-radius: 3px;
  }
</style>
