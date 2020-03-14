<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" @click="nextImage">Next</button>
      <button class="m-1 order-sm-1" type="button" :disabled="!hasPreviousImage" @click="previousImage">Previous</button>
    </div>
    <div id="image-post" class="p-1" v-if="currentImage.imageUri">
      <image-post-image class="mt-3" max-height="50vh" :imageUri="currentImage.imageUri"/>
      <div id="image-details" class="d-flex flex-column flex-md-row justify-content-between">
        <div class="my-3 order-md-2 text-center text-md-right">
          <div>posted by {{ currentImageUsername }}</div>
          <div>{{ formattedDateCreated }}</div>
        </div>
        <image-post-feedback class="my-3 justify-content-center text-center text-md-left order-md-1"/>
      </div>
      <div class="my-3 text-center">
        {{ currentImage.description }}
      </div>
    </div>
  </div>
</template>

<script>
import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'

export default {
  name: "Explore",
  props: {
    isLoggedIn: Boolean,
    imagePostService: Object,
    randomImagePostService: Object
  },
  components: {
    ImagePostImage,
    ImagePostFeedback
  },
  data: function(){
    return {
      images: [],
      currentImageIndex: 0
    }
  },
  computed: {
    hasPreviousImage: function() {
      return this.currentImageIndex > 0;
    },
    currentImage: function() {
      return this.images[this.currentImageIndex] || {};
    },
    formattedDateCreated: function() {
      return this.currentImage.dateCreated ? this.currentImage.dateCreated.toLocaleDateString() : null
    },
    currentImageUsername: function() {
      return this.currentImage.user ? this.currentImage.user.username : null
    }
  },
  created: function() {
    const initialImageId = this.$route.params.initialId;
    
    if(initialImageId) {
      this.imagePostService.getById(initialImageId).then(image => this.images.push(image))
    } else {
      this.randomImagePostService.getRandom().then(image => this.images.push(image));
    }
  },
  methods: {
    nextImage: async function() {
      if(this.isShowingLastImage) {
        let newImage = await this.randomImagePostService.getRandom();
        this.images.push(newImage);
      }

      this.currentImageIndex++;
    },
    previousImage: function() {
      if(this.hasPreviousImage) {
        this.currentImageIndex--;
      }
    },
    isShowingLastImage: function() {
      return this.currentImageIndex + 1 == this.images.length
    }
  }
};
</script>

<style scoped>
  #image-post {
    max-width: 700px;
    margin: auto;
  }
</style>
