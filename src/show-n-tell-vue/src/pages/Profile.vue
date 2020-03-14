<template>
    <div>
        <h1 class="text-center">{{ username }}</h1>
        <h3 class="mt-4 text-center text-lg-left">Image Posts</h3>
        <ul class="row justify-content-center justify-content-lg-start">
            <li class="col-lg-4 d-flex flex-column mt-5" v-for="post in imagePosts" :key="post.id">
                <image-post-image class="img-post-img" max-height="50vh" @click="() => viewImagePost(post.id)" :imageUri="post.imageUri"/>
                <div class="d-flex flex-column flex-sm-row align-items-center justify-content-sm-between">
                    <image-post-feedback />
                    <more-dropdown>
                        <ul class="my-dropdown">
                            <li @click="() => viewImagePost(post.id)" class="px-3 py-2 my-dropdown-item">View</li>
                            <li class="px-3 py-2 my-dropdown-item">Edit</li>
                            <li class="px-3 py-2 my-dropdown-item">Delete</li>
                        </ul>
                    </more-dropdown>
                </div>
            </li>
        </ul>
    </div>
</template>

<script>
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
        profileService: Object,
        userService: Object
    },
    data: function() {
        return {
            imagePosts: [],
            username: ""
        }
    },
    created: function() {
        const user = this.userService.getUser()

        if(user) {
            this.username = user.username
            this.profileService.getImagePosts(this.username).then(posts => {
                this.imagePosts = posts.sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))
            });
        }
    },
    methods: {
        viewImagePost: function(imagePostId) {
            this.$router.push({path: `explore/${imagePostId}`})
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