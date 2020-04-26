<template>
    <div class="d-flex flex-wrap">
        <div class="d-flex align-items-center justify-content-center">
            <div :class="{ 'like-button': canLike}">
                <img v-if="!isLikedData" @click="liked" src="../../assets/icons/like-white.png"/>
                <img v-else @click="unliked" src="../../assets/icons/like-black.png"/>
            </div>
            <div class="ml-1">{{ likeCountData }}</div>
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
    data: function(){
        return {
            isLikedData: this.isLiked,
            likeCountData: this.likeCount
        }
    },
    watch: {
        isLiked: function() {
            this.isLikedData = this.isLiked
        },
        likeCount: function() {
            this.likeCountData = this.likeCount
        }
    },
    methods: {
        liked: function() {
            if(this.canLike) {
                this.$emit('liked')
                this.isLikedData = true
                this.likeCountData++;
            }
        },
        unliked: function() {
            if(this.canLike) {
                this.$emit('unliked')
                this.isLikedData = false
                this.likeCountData--;
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