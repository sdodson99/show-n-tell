<template>
    <div class="d-flex flex-wrap">
        <div class="d-flex align-items-center justify-content-center">
            <img v-if="!liked" @click="toggleLiked" src="../../assets/icons/like-white.png"/>
            <img v-else @click="toggleLiked" src="../../assets/icons/like-black.png"/>
            <div class="ml-1">{{ likeCount }}</div>
        </div>
        <div class="ml-3 d-flex align-items-center justify-content-center">
            <img class="mt-2" src="../../assets/icons/comment.png"/>
            <div class="ml-1">{{ commentCount }}</div>
        </div>
    </div>
</template>

<script>
export default {
    name: "ImagePostFeedback",
    props: {
        liked: {
            type: Boolean,
            default: function() {
                return false
            }
        },
        likes: {
            type: Array,
            default: function() {
                return []
            }
        },
        comments: {
            type: Array,
            default: function() {
                return []
            }
        }
    },
    computed: {
        likeCount: function(){
            return this.likes.length
        },
        commentCount: function(){
            return this.comments.length
        }
    },
    methods: {
        toggleLiked: function() {
            this.$emit(this.liked ? 'unliked' : 'liked')
        }
    }
}
</script>

<style>

</style>