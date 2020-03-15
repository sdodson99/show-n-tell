<template>
    <div>
        <h1 class="text-center">{{ username }}</h1>
        <h3 class="mt-4 text-center text-lg-left">Image Posts</h3>
        <div v-if="hasNoImagePosts" class="mt-4 text-center">
            You have not posted any images yet.
        </div>
        <ul v-else class="row justify-content-center justify-content-lg-start">
            <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in imagePosts" :key="post.id">
                <image-post-image class="img-post-img" max-height="30vh" @click="() => viewImagePost(post.id)" :imageUri="post.imageUri"/>
                <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
                    <image-post-feedback />
                    <more-dropdown>
                        <ul class="my-dropdown">
                            <li @click="() => viewImagePost(post.id)" class="px-3 py-2 my-dropdown-item">View</li>
                            <li @click="() => editImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Edit</li>
                            <li @click="() => deleteImagePost(post.id)" class="px-3 py-2 my-dropdown-item">Delete</li>
                        </ul>
                    </more-dropdown>
                </div>
            </li>
        </ul>
    </div>
</template>

<script>
import UnauthorizedError from '../errors/unauthorized-error'

import ImagePostImage from '../components/image-posts/ImagePostImage'
import ImagePostFeedback from '../components/image-posts/ImagePostFeedback'
import MoreDropdown from '../components/utilities/MoreDropdown'

export default {
    name: "Profile",
    components: {
        ImagePostImage,
        ImagePostFeedback,
        MoreDropdown
    },
    props: {
        imagePostService: Object,
        profileService: Object,
        userService: Object
    },
    data: function() {
        return {
            imagePosts: [],
            username: "",
            isLoaded: false
        }
    },
    computed: {
        hasNoImagePosts: function() {
            return this.isLoaded && this.imagePosts.length === 0
        }
    },
    created: function() {
        const user = this.userService.getUser()

        if(user) {
            this.username = user.username
            this.profileService.getImagePosts(this.username).then(posts => {
                this.imagePosts = posts.sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))
                this.isLoaded = true;
            });
        }
    },
    methods: {
        viewImagePost: function(imagePostId) {
            this.$router.push({path: `explore/${imagePostId}`})
        },
        editImagePost: function(imagePostId) {
            this.$router.push({path: `imagePosts/${imagePostId}/edit`})
        },
        deleteImagePost: async function(imagePostId) {
            try {
                const success = await this.imagePostService.delete(imagePostId)

                if(success) {
                    this.imagePosts = this.imagePosts.filter(p => p.id !== imagePostId)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError) {
                    this.$router.push({path: "/login"})
                }
            }
        }
    }
}
</script>

<style scoped>
ul {
    list-style: none;
}

.img-post-img {
    cursor: pointer;
}

.my-dropdown{
    border: 1px solid var(--color-primary-dark);
    border-radius: 3px;
    background: white;
}

.my-dropdown-item{
    white-space: nowrap;
    cursor: pointer;
}

.my-dropdown-item:hover {
    background: var(--color-grayscale-light);
}

</style>