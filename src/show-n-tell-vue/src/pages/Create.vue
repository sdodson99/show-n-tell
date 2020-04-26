<template>
  <div class="d-flex flex-column">
    <h1 class="text-center">Create a Show 'N Tell Image</h1>
    <form id="create-form" class="my-5 d-flex flex-column">
      <div class="my-3">
        <h3 class="mx-1 text-center">Image</h3>
        <div class="m-1 custom-file">
          <label class="custom-file-label" for="inputGroupFile01">{{ imageName }}</label>
          <input id="inputGroupFile01" class="custom-file-input" type="file" 
            @change="handleImageChange"
            :disabled="isCreating">
        </div>
      </div>
      <div class="my-3">
        <h3 class="mx-1 text-center">Description</h3>
        <textarea class="m-1 form-control" rows="3" placeholder="Once upon a time..." 
          v-model="description"
          :disabled="isCreating"></textarea>
      </div>
      <div class="my-3">
        <h3 class="mx-1 text-center">Tags</h3>
        <textarea class="m-1 form-control" rows="1" placeholder="Tags (separate with comma)" 
          v-model="tags"
          :disabled="isCreating"></textarea>
      </div>
      <button class="m-1 p-3 align-self-center" type="button" 
        @click="createImage"
        :disabled="isCreating || !canCreate">Create</button>
      <div class="text-center"
          v-if="isCreating">
          <b-spinner class="mt-4 text-center" label="Creating..."></b-spinner>
      </div>
    </form>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { ModuleName, Action } from '../store/modules/create/types'

export default {
    name: "Create",
    data: function() {
      return {
        image: null,
        description: "",
        tags: ""
      }
    },
    computed: {
      ...mapState({
        isCreating: (state) => state.create.isCreating
      }),
      imageName: function() {
        return this.image ? this.image.name : "Choose File";
      },
      canCreate: function() {
        return this.image != null
      }
    },
    methods: {
      createImage: function() {
        const newImagePost = {
          image: this.image,
          description: this.description,
          tags: this.tags.split(",").map(t => t.trim())
        }

        this.$store.dispatch(`${ModuleName}/${Action.CREATE_IMAGE_POST}`, newImagePost)
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