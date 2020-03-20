<template>
    <div class="d-flex flex-wrap">
        <div class="d-flex align-items-center justify-content-center">
            <div :class="likeButtonClass">
                <img v-if="!isLiked" @click="toggleLiked" src="../../assets/icons/like-white.png"/>
                <img v-else @click="toggleLiked" src="../../assets/icons/like-black.png"/>
            </div>
            <div class="ml-1">{{ numLikes }}</div>
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
        liked: Boolean,
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
            isLiked: this.liked,
            numLikes: this.likeCount
        }
    },
    computed: {
        likeButtonClass: function(){
            return this.canLike ? "like-button" : ""
        }
    },
    watch: {
        liked: function() {
            this.isLiked = this.liked
        },
        likeCount: function() {
            this.numLikes = this.likeCount
        }
    },
    methods: {
        toggleLiked: function() {
            if(this.canLike) {
                this.isLiked = !this.isLiked
                this.isLiked ? this.numLikes++ : this.numLikes--
                this.$emit(this.isLiked ? 'liked' : 'unliked')
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