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
      v-if="currentImage.imageUri">
      <image-post-detailed-image class="mt-3"
          maxImagePostHeight="50vh"
          :imagePost="currentImage"
          :imagePostService="imagePostService"
          :likeVueService="likeVueService"
          :currentUser="currentUser"/>
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
      v-else>
      <h3 class="text-center">{{ noImageMessage }}</h3>
    </div>
  </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'

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
      canViewNext: true
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
      const image = await this.imagePostService.getById(initialImageId);
  
      if(image) {
        this.images.push(image)
      } else {
        this.noImageMessage = "The selected image does not exist."
      }
    // If no initial id is provided, show a random image.
    } else {
      const image = await this.randomImagePostService.getRandom();     

      if(image) {
        this.images.push(image)
        this.$route.params.initialId = image.id
      } else {
        this.noImageMessage = "No images have been posted."
        this.canViewNext = false;
      }
    }
  },
  methods: {
    nextImage: async function() {
      // If the last image is being shown, we need to ask for another image.
      if(this.isShowingLastImage) {
        let newImage = await this.randomImagePostService.getRandom();

        // If the new image already exists in the history, add the already existing image.
        let existingImage = this.images.find(p => p.id === newImage.id)
        if(existingImage) {
          this.images.push(existingImage)
        } else {
          this.images.push(newImage)
        }
      }
      
      this.currentImageIndex++;
    },
    previousImage: function() {
      if(this.hasPreviousImage) {
        this.currentImageIndex--;
      }
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
