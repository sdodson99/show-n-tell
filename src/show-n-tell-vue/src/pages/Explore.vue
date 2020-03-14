<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" @click="nextImage">Next</button>
      <button class="m-1 order-sm-1" type="button" :disabled="!hasPreviousImage" @click="previousImage">Previous</button>
    </div>
    <div id="image-post" class="p-1" v-if="currentImage.imageUri">
      <div id="image-container" class="mt-3 text-center">
        <img id="explore-image" :src="currentImage.imageUri"/>
      </div>
      <div id="image-details" class="d-flex flex-column flex-md-row justify-content-between">
        <div class="my-3 order-md-2 text-center text-md-right">
          <div>posted by {{ currentImage.userEmail }}</div>
          <div>{{ formattedDateCreated }}</div>
        </div>
        <div class="my-3 d-flex flex-wrap order-md-1 justify-content-center text-center text-md-left">
          <div class="d-flex align-items-center justify-content-center justify-content-md-start">
            <img src="../assets/icons/like-white.png"/>
            <div class="ml-1">{{ currentImage.likes }} likes</div>
          </div>
          <div class="ml-3 d-flex align-items-center justify-content-center justify-content-md-start">
            <img class="mt-2" src="../assets/icons/comment.png"/>
            <div class="ml-1">{{ currentImage.comments }} comments</div>
          </div>
        </div>
      </div>
      <div class="my-3 text-center">
        {{ currentImage.description }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Explore",
  props: {
    isLoggedIn: Boolean,
    randomImagePostService: Object
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
    }
  },
  created: function() {
    this.randomImagePostService.getRandom().then(image => this.images.push(image));
  },
  methods: {
    nextImage: async function() {
      if(this.isShowingLastImage) {
        let newImage = await this.randomImagePostService.getRandom();
        newImage.likes = 143;
        newImage.comments = 5;
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
  #image-container{
    background: var(--color-grayscale-light);
    border-radius: 3px;
    border: 1px solid var(--color-primary-dark);
  }

  #explore-image{
    max-height: 50vh;
    max-width: 100%;
    background: white;
  }

  #image-post {
    max-width: 700px;
    margin: auto;
  }
</style>
