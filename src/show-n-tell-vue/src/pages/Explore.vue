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
      <image-post-image class="mt-3" max-height="50vh" 
        :imageUri="currentImage.imageUri"/>
      <div id="image-details" class="d-flex flex-column flex-md-row justify-content-between">
        <div class="my-3 order-md-2 text-center text-md-right">
          <div>posted by 
            <a @click="viewProfile">{{ currentImageUsername }}</a>
          </div>
          <div>{{ formattedDateCreated }}</div>
        </div>
        <image-post-feedback class="my-3 justify-content-center text-center text-md-left order-md-1"
          :canLike="!isUsersPost"
          :liked="isLiked"
          :likeCount="currentImage.likes.length"
          @liked="likeImage"
          @unliked="unlikeImage"/>
      </div>
      <div class="text-center">
        <p>{{ currentImage.description }}</p>
      </div>
      <div class="my-4 text-center text-sm-left">
        <h3>Comments</h3>
      </div>
    </div>
    <div class="mt-3"
      v-else>
      <h3 class="text-center">{{ noImageMessage }}</h3>
    </div>
  </div>
</template>

<script>
import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'
import UnauthorizedError from '../errors/unauthorized-error'
import LikeServiceMixin from '../mixins/like-service-mixin'

export default {
  name: "Explore",
  props: {
    currentUser: Object,
    imagePostService: Object,
    randomImagePostService: Object,
    likeService: Object
  },
  components: {
    ImagePostImage,
    ImagePostFeedback
  },
  mixins: [
    LikeServiceMixin
  ],
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
    currentImageUsername: function() {
      return this.currentImage.user.username
    },
    isShowingLastImage: function() {
      return this.currentImageIndex + 1 === this.images.length
    },
    hasPreviousImage: function() {
      return this.currentImageIndex > 0;
    },
    isLiked: function() {
      return this.currentImage.likes.some(l => l.userEmail === (this.currentUser && this.currentUser.email))
    },
    isUsersPost: function() {
      return this.currentImage.userEmail === (this.currentUser && this.currentUser.email)
    },
    formattedDateCreated: function() {
      return this.currentImage.dateCreated.toLocaleDateString()
    },
  },
  created: async function() {    
    const initialImageId = this.$route.params.initialId;
    
    if(initialImageId) {
      const image = await this.imagePostService.getById(initialImageId);

      if(image) {
        this.images.push(image)
      } else {
        this.noImageMessage = "The selected image does not exist."
      }
    } else {
      const image = await this.randomImagePostService.getRandom();      

      if(image) {
        this.images.push(image)
      } else {
        this.noImageMessage = "No images have been posted."
        this.canViewNext = false;
      }
    }
  },
  methods: {
    nextImage: async function() {
      if(this.isShowingLastImage) {
        let newImage = await this.randomImagePostService.getRandom();

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
    viewProfile: function() {
      this.$router.push({path: `/profile/${this.currentImageUsername}`})
    },
    likeImage: async function() {
      this.currentImage.likes = await this._likeImage(this.currentImage)
    },
    unlikeImage: async function() {
      this.currentImage.likes = await this._unlikeImage(this.currentImage)
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

  a, a:hover{
    color: unset;
    text-decoration: underline;
    cursor: pointer;
  }
</style>
