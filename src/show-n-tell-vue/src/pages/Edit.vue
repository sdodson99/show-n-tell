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
import { ModuleName, Action, Mutation } from '../store/modules/edit/types'

import ImagePostImage from '../components/image-posts/ImagePostImage'

export default {
    name: "Edit",
    components: {
        ImagePostImage
    },
    computed: {
      ...mapState({
        imageUri: (state) => state.edit.imageUri,
        isLoading: (state) => state.edit.isLoading,
        isUpdating: (state) => state.edit.isUpdating
      }),
      description: {
        get() {
          return this.$store.state.edit.description
        }, 
        set(value) {
          this.$store.commit(`${ModuleName}/${Mutation.SET_DESCRIPTION}`, value)
        }
      },
      tags: {
        get() {
          return this.$store.state.edit.tags
        },
        set(value) {
          this.$store.commit(`${ModuleName}/${Mutation.SET_TAGS}`, value)
        }
      }
    },
    created: function() {
        this.$store.dispatch(`${ModuleName}/${Action.SET_IMAGE_POST_BY_ID}`, this.$route.params.imagePostId)
    },
    methods: {
        updateImagePost: function() {
          this.$store.dispatch(`${ModuleName}/${Action.UPDATE_IMAGE_POST}`)
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