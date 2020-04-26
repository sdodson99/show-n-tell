<template>
  <div class="d-flex flex-column">
    <h1 class="text-center">Edit a Show 'N Tell Image</h1>
    <form id="edit-form" class="my-5 d-flex flex-column">
      <div class="my-3">
        <h3 class="mx-1 text-center">Image</h3>
        <image-post-image max-height="25vh" :imageUri="imageUri"/>
      </div>
      <div class="my-3">
        <h3 class="mx-1 text-center">Description</h3>
        <textarea class="m-1 form-control" placeholder="Once upon a time..." 
          v-model="description"
          :disabled="isUpdating"></textarea>
      </div>
      <div class="my-3">
        <h3 class="mx-1 text-center">Tags</h3>
        <textarea class="m-1 form-control" rows="1" placeholder="Tags (separate with comma)" 
          v-model="tags"
          :disabled="isUpdating"></textarea>
      </div>
      <button class="m-1 p-3 align-self-center" type="button" 
        @click="updateImagePost"
        :disabled="isUpdating">Save</button>
      <div class="text-center"
          v-if="isUpdating">
          <b-spinner class="mt-4 text-center" label="Updating..."></b-spinner>
      </div>
    </form>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { ModuleName, Action, Mutation } from '../store/modules/image-posts/types'

import ImagePostImage from '../components/image-posts/ImagePostImage'

export default {
    name: "Edit",
    components: {
        ImagePostImage
    },
    data: function() {
      return {
        imagePostId: 0,
        imageUri: "",
        description: "",
        tags: "",
        isUpdating: false,
        isLoading: false
      }
    },
    computed: {
      ...mapState({
        imagePosts: (state) => state.imagePosts.imagePosts
      })
    },
    created: async function() {
      this.isLoading = true;

      this.imagePostId = this.$route.params.imagePostId
      await this.$store.dispatch(`${ModuleName}/${Action.FETCH_IMAGE_POST_BY_ID}`, this.imagePostId)

      this.imageUri = this.imagePosts[this.imagePostId].imageUri
      this.description = this.imagePosts[this.imagePostId].description
      this.tags = this.imagePosts[this.imagePostId].tags.join(',')

      this.isLoading = false
    },
    methods: {
      updateImagePost: async function() {
        this.isUpdating = true;

        const imagePost = {
          description: this.description,
          tags: this.tags.split(",").map(t => t.trim())
        }

        await this.$store.dispatch(`${ModuleName}/${Action.UPDATE_IMAGE_POST}`, { imagePostId: this.imagePostId, imagePost })

        this.isUpdating = false;
    }
  }
}
</script>

<style scoped>
#edit-form {
    width: 100%;
    max-width: 600px;
    margin: auto;
}
</style>