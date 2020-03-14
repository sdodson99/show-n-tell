<template>
    <div class="d-flex flex-wrap">
        <div class="d-flex align-items-center justify-content-center">
            <img src="../../assets/icons/like-white.png"/>
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
        likes: {
            type: Array,
            default: function() {
                return [1,2,3,4,5]
            }
        },
        comments: {
            type: Array,
            default: function() {
                return [1,2]
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
    }
}
</script>

<style>

</style>