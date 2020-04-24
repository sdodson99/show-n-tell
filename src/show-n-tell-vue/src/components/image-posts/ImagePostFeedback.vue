<template>
    <div class="d-flex flex-wrap">
        <div class="d-flex align-items-center justify-content-center">
            <div :class="{ 'like-button': canLike}">
                <img v-if="!isLiked" @click="liked" src="../../assets/icons/like-white.png"/>
                <img v-else @click="unliked" src="../../assets/icons/like-black.png"/>
            </div>
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
        isLiked: Boolean,
        canLike: Boolean,
        likeCount: {
            type: Number,
            default: 0
        },
        commentCount: {
            type: Number,
            default: 0
        }
    },
    methods: {
        liked: function() {
            if(this.canLike) {
                this.$emit('liked')
            }
        },
        unliked: function() {
            if(this.canLike) {
                this.$emit('unliked')
            }
        }
    }
}
</script>

<style>
.like-button{
    cursor: pointer;
}
</style>