<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" 
        :disabled="!canViewNext" 
        @click="nextImage">Next</button>
      <button class="m-1 order-sm-1" type="button" 
        :disabled="!hasPreviousImage" 
        @click="previousImage">Previous</button>
    </div>
    <div id="image-post" class="p-1" 
      v-if="!isLoading && currentImage.imageUri">
      <image-post-detailed-image class="mt-3"
          maxImagePostHeight="50vh"
          :imagePost="currentImage"
          :imagePostService="imagePostService"
          :likeVueService="likeVueService"
          :currentUser="currentUser"
          :canView="false"
          @imagePostDeleted="imagePostDeleted"/>
      <div class="my-4">
        <image-post-comment class="text-center text-sm-left"
          :content="currentImage.description"
          fallbackContent="No description available."
          :username="currentImage.username"
          :dateCreated="currentImage.dateCreated"
          @usernameClicked="(username) => viewProfile(username)"/>
      </div>
      <div class="my-5 text-center text-sm-left">
        <h3>Comments</h3>
        <image-post-comment-list class="mt-3" 
          :comments="currentImage.comments"
          :can-comment="isLoggedIn"
          @commented="createComment"
          @usernameClicked="viewProfile"/>
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
import UnauthorizedError from '../errors/unauthorized-error'
import NotFoundError from '../errors/not-found-error'

import ImagePostComment from '../components/image-posts/ImagePostComment'
import ImagePostCommentList from '../components/image-posts/ImagePostCommentList'
import ImagePostDetailedImage from '../components/image-posts/ImagePostDetailedImage'

export default {
  name: "Explore",
  props: {
    imagePostService: Object,
    randomImagePostService: Object,
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
  data: function(){
    return {
      images: [],
      currentImageIndex: 0,
      noImageMessage: "",
      canViewNext: true,
      isLoading: true
    }
  },
  computed: {
    currentImage: function() {
      return this.images[this.currentImageIndex] || {};
    },
    isShowingLastImage: function() {
      return this.currentImageIndex + 1 === this.images.length
    },
    hasPreviousImage: function() {
      return this.currentImageIndex > 0;
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
    
    // If initial id is provided, show the image with the id.
    if(initialImageId) {
      await this.getImage(initialImageId)
    // If no initial id is provided, show a random image.
    } else {
      await this.getRandomImage()
    }
  },
  methods: {
    getImage: async function(id) {
      this.isLoading = true

      try {
        const image = await this.imagePostService.getById(id);
        
        this.images.push(image)
      } catch (error) {
        this.noImageMessage = "The selected image does not exist."
        this.currentImageIndex = -1
      }

      this.isLoading = false
    },
    getRandomImage: async function() {
      this.isLoading = true

      try {
        let newImage = await this.randomImagePostService.getRandom();
        
        let existingImage = this.images.find(p => p.id === newImage.id)

        // If the new image already exists in the history, add the already existing image.
        if(existingImage) {
          this.images.push(existingImage)
        } else {
          this.images.push(newImage)
        }
      } catch (error) {
        this.disableViewNextImage()
      }

      this.isLoading = false
    },
    nextImage: async function() {
      // If the last image is being shown, we need to ask for another image.
      if(this.isShowingLastImage) {
        await this.getRandomImage()
      }
      
      this.currentImageIndex++;
    },
    previousImage: function() {
      let success = false

      if(this.hasPreviousImage) {
        this.currentImageIndex--;
        success = true
      }

      return success
    },
    viewProfile: function(username) {
      this.$router.push({path: `/profile/${username}`})
    },
    likeImage: async function() {
      this.currentImage.likes = await this.likeVueService.likeImagePost(this.currentImage, this.currentImagePostRoute)
    },
    unlikeImage: async function() {
      this.currentImage.likes = await this.likeVueService.unlikeImagePost(this.currentImage, this.currentImagePostRoute)
    },
    createComment: async function(comment) {
      this.currentImage.comments = await this.commentVueService.createComment(this.currentImage, comment, this.currentImagePostRoute)
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
    disableViewNextImage: function () {
      this.noImageMessage = "No images have been posted."
      this.canViewNext = false;
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
