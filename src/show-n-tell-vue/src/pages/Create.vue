<template>
  <div class="d-flex flex-column">
    <h1 class="text-center">Create a Show 'N Tell Image</h1>
    <form id="create-form" class="my-5 d-flex flex-column">
      <div class="my-3">
        <h3 class="mx-1 text-center">Image</h3>
        <div class="m-1 custom-file">
          <label class="custom-file-label" for="inputGroupFile01">{{ imageName }}</label>
          <input id="inputGroupFile01" class="custom-file-input" type="file" @change="handleImageChange">
        </div>
      </div>
      <div class="my-3">
        <h3 class="mx-1 text-center">Description</h3>
        <textarea class="m-1 form-control" placeholder="Once upon a time..." v-model="description"></textarea>
      </div>
      <button class="m-1 p-3 align-self-center" type="button" @click="createImage">Create</button>
    </form>
  </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'

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

        try{
          const createdImage = await this.imagePostService.create(newImage);

          if(createdImage) {
            this.$router.push({path: "/profile"})
          }
        } catch (error) {
              if(error instanceof UnauthorizedError) {
                  this.$router.push({path: "/login"})
              }
          }
      },
      handleImageChange: function(event) {
        const file = event.target.files[0];
        this.image = file;
      }
    }
}
</script>

<style scoped>
  #create-form {
    width: 100%;
    max-width: 600px;
    margin: auto;
  }

  input[type="file"] {
    border-radius: 3px;
  }
</style>