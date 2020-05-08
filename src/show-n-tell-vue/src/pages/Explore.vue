<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" 
        :disabled="!canExplore" 
        @click="viewNext">Next</button>
      <button class="m-1 order-sm-1" type="button" 
        :disabled="!hasPreviousImagePost" 
        @click="viewPrevious">Previous</button>
    </div>
    <div id="image-post" class="p-1" 
      v-if="!isLoading && currentImagePost && currentImagePost.imageUri">
      <image-post-detailed-image class="mt-3"
          :image-post="currentImagePost"
          :current-user="currentUser"
          :can-view="false"
          maxImagePostHeight="50vh"
          @liked="likeImagePost"
          @unliked="unlikeImagePost"
          @deleted="deleteImagePost"/>
      <div class="my-4">
        <image-post-comment
          :content="currentImagePost.description"
          :canEdit="false"
          :canDelete="false"
          fallbackContent="No description available."
          :username="currentImagePost.username"
          :dateCreated="currentImagePost.dateCreated"
          @username-clicked="viewProfile"/>
      </div>
      <div class="my-4">
        <h3>Comments</h3>
        <image-post-comment-list class="mt-3" 
          :comments="currentImagePost.comments"
          :currentUser="currentUser"
          :imagePostUserEmail="currentImagePost.userEmail"
          :can-comment="isLoggedIn"
          @username-clicked="viewProfile"
          @created="createImagePostComment"
          @edited="editImagePostComment"
          @deleted="deleteImagePostComment"/>
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
import { ModuleName as ExploreModuleName, Action as ExploreAction, Mutation } from '../store/modules/explore/types'
import { ModuleName as ImagePostsModuleName, Action as ImagePostsAction } from '../store/modules/image-posts/types'
import { ModuleName as AuthenticationModuleName } from '../store/modules/authentication/types'

import ImagePostComment from '../components/image-posts/ImagePostComment'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetailedImage from '../components/image-posts/ImagePostDetailedImage'

export default {
  name: "Explore",
  components: {
    ImagePostComment,
    ImagePostCommentList,
    ImagePostDetailedImage
  },
  computed: {
    ...mapState({
      isLoading: (state) => state.explore.isLoading,
      noImagePostsAvailable: (state) => state.explore.noImagePostsAvailable,
      imagePostNotFound: (state) => state.explore.imagePostNotFound,
      currentUser: (state) => state.user.currentUser
    }),
    ...mapGetters(ExploreModuleName, ['currentImagePost', 'noImagePosts', 'canExplore', 'hasPreviousImagePost']),
    ...mapGetters(AuthenticationModuleName, ['isLoggedIn']),
    noImageMessage: function() {
      if(this.noImagePostsAvailable) return "No images have been posted."
      if(this.imagePostNotFound) return "The selected image does not exist."
      return ""
    }
  },
  created: function() {
    const initialImageId = this.$route.params.initialId;
    
    if(initialImageId) {
      this.$store.dispatch(`${ExploreModuleName}/${ExploreAction.VIEW_IMAGE_POST_BY_ID}`, initialImageId)
    } else {
      if(this.noImagePosts) {
        this.$store.dispatch(`${ExploreModuleName}/${ExploreAction.VIEW_RANDOM_IMAGE_POST}`)
      }
    }
  },
  methods: {
    viewNext: function() {
      this.$store.dispatch(`${ExploreModuleName}/${ExploreAction.NEXT_IMAGE_POST}`)
    },
    viewPrevious: function() {
      this.$store.dispatch(`${ExploreModuleName}/${ExploreAction.PREVIOUS_IMAGE_POST}`)
    },
    likeImagePost: function() {
      this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.LIKE_IMAGE_POST}`, this.currentImagePost)
    },
    unlikeImagePost: function() {
      this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.UNLIKE_IMAGE_POST}`, this.currentImagePost)
    },
    deleteImagePost: function() {
      this.$store.dispatch(`${ExploreModuleName}/${ExploreAction.DELETE_IMAGE_POST}`)
    }, 
    createImagePostComment: function(content) {
      this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.CREATE_IMAGE_POST_COMMENT}`, { imagePost: this.currentImagePost, content })
    },
    editImagePostComment: function(comment) {
      this.$store.dispatch(`${ImagePostsModuleName}/${ImagePostsAction.UPDATE_IMAGE_POST_COMMENT}`, { imagePost: this.currentImagePost, comment })
    },
    deleteImagePostComment: function(commentId) {
      this.$store.dispatch(`${ImagePostsModuleName}/${ExploreAction.DELETE_IMAGE_POST_COMMENT}`, { imagePost: this.currentImagePost, commentId })
    },
    viewProfile: function(username) {
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
