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
        @click="saveImagePost"
        :disabled="isUpdating">Save</button>
      <div class="text-center"
          v-if="isUpdating">
          <b-spinner class="mt-4 text-center" label="Updating..."></b-spinner>
      </div>
    </form>
  </div>
</template>

<script>
import ImagePostImage from '../components/image-posts/ImagePostImage'
import UnauthorizedError from '../errors/unauthorized-error'

export default {
    name: "Edit",
    components: {
        ImagePostImage
    },
    props: {
        imagePostService: Object
    },
    data: function() {
        return {
            imagePostId: 0,
            imageUri: null,
            description: "",
            tags: "",
            isUpdating: false
        }
    },
    created: async function() {
        this.imagePostId = this.$route.params.imagePostId
        const currentImagePost = await this.imagePostService.getById(this.imagePostId)

        this.imageUri = currentImagePost.imageUri
        this.description = currentImagePost.description
        this.tags = currentImagePost.tags.join(',')
    },
    methods: {
        saveImagePost: async function() {
            this.isUpdating = true

            try {
                const currentImageId = this.imagePostId
                const updateImageRequest = {
                    description: this.description,
                    tags: this.tags.split(",").map(t => t.trim())
                }

                const updatedImage = await this.imagePostService.update(currentImageId, updateImageRequest)

                if(updatedImage.id) {
                    this.$router.push({path: `/explore/${updatedImage.id}`})
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                  this.$router.push({path: "/login", query: { back: true }})
                }
            }

            this.isUpdating = false
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