<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button" @click="nextImage">Next</button>
      <button class="m-1 order-sm-1" type="button" :disabled="!hasPreviousImage" @click="previousImage">Previous</button>
    </div>
    <div class="my-3 text-center">
      <img id="explore-image" src="https://images.pexels.com/photos/255379/pexels-photo-255379.jpeg"/>
    </div>
    <div class="my-1 text-center">
      <div>posted by {{ currentImage.userEmail }}</div>
      <div>{{ currentImage.dateCreated }}</div>
    </div>
    <div class="my-3 text-center">
      {{ currentImage.description }}
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
    }
  },
  created: function() {
    this.randomImagePostService.getRandom().then(image => this.images.push(image));
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
  #explore-image{
    border: 1px solid var(--color-primary-dark);
    border-radius: 3px;
    max-height: 50vh;
    max-width: 100%;
  }

  button{
    border: none;
    padding: 1em;
    background: var(--color-secondary-medium);
    color: var(--color-primary-dark);
    border-radius: 5px;
    min-width: 100px;
    cursor: pointer;
  }

  button:disabled{
    border: 1px solid var(--color-secondary-medium);
    background: var(--color-grayscale-light);
    cursor: default;
  }

</style>
