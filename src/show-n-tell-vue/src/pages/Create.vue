<template>
  <div class="d-flex flex-column align-items-center">
    <h1 class="text-center">Create a Show 'N Tell Image</h1>
    <form class="m-5 d-flex flex-column">
      <div class="m-3">
        <h3 class="mx-1 text-center">Image</h3>
        <div class="m-1 custom-file">
          <label class="custom-file-label" for="inputGroupFile01">{{ imageName }}</label>
          <input id="inputGroupFile01" class="custom-file-input" type="file" @change="handleImageChange">
        </div>
      </div>
      <div class="m-3">
        <h3 class="mx-1 text-center">Description</h3>
        <textarea class="m-1 form-control" placeholder="Once upon a time..." v-model="description"></textarea>
      </div>
      <button class="m-5 p-2 align-self-center" type="button" @click="createImage">Create</button>
    </form>
  </div>
</template>

<script>
export default {
    name: "Create",
    props: {
      imagePostService: Object
    },
    data: function() {
      return {
        image: null,
        description: ""
      }
    },
    computed: {
      imageName: function() {
        return this.image ? this.image.name : "Choose File";
      },
    },
    methods: {
      createImage: async function() {
        const newImage = {
          image: this.image,
          description: this.description
        }

        const createdImage = await this.imagePostService.create(newImage);

        console.log(createdImage);
      },
      handleImageChange: function(event) {
        const file = event.target.files[0];
        this.image = file;
      }
    }
}
</script>

<style scoped>
  input[type="file"] {
    /* border: 1px solid var(--color-primary-dark); */
    border-radius: 3px;
  }
</style>