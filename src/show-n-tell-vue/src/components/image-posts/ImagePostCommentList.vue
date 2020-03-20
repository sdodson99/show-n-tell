<template>
    <div>
        <div id="new-comment-form" class="d-flex flex-column align-items-center align-items-sm-end"
            v-if="canComment">
            <textarea class="form-control" placeholder="New comment..."
                v-model="newCommentContent"></textarea>
            <button type="button" class="my-3" 
                :disabled="!validComment"
                @click="submit">Submit</button>
         </div>
         <div class="text-center"
            v-else>
            You must be logged in to comment.
         </div>
         <ul v-if="hasComments">
             <li class="comment-item py-3"
                v-for="comment in comments"
                :key="comment.id">
                <div class="d-flex flex-column flex-sm-row">
                    <div class="username font-weight-bold"
                        @click="() => usernameClicked(comment.username)">
                        {{ comment.username }}
                    </div>
                    <div class="mx-3 d-none d-sm-block">|</div>
                    <div>
                        {{ getFormattedDateCreated(comment.dateCreated) }}
                    </div>
                </div>
                <div class="mt-2">
                    {{ comment.content }}
                </div>
             </li>
         </ul>
         <div class="mt-2 text-center" 
            v-else>
             No comments have been posted.
         </div>
    </div>
</template>

<script>
export default {
    name: "ImagePostCommentList",
    props: {
        comments: {
            type: Array,
            default: function() {
                return []
            }
        },
        canComment: {
            type: Boolean,
            default: false
        }
    },
    data: function() {
        return {
            newCommentContent: ""
        }
    },
    computed: {
        hasComments: function() {
            return this.comments.length > 0
        },
        validComment: function() {
            return this.newCommentContent
        }
    },
    methods: {
        submit: function() {
            if(this.validComment) {
                this.$emit('commented', this.newCommentContent)
            }
        },
        getFormattedDateCreated: function(date) {
            return new Date(date).toLocaleDateString()
        },
        usernameClicked: function(username) {
            this.$emit('usernameClicked', username)
        }
    }
}
</script>

<style scoped>
textarea {
    min-width: 100%;
}

ul {
    list-style: none;
}

.comment-item {
    border-top: 1px solid var(--color-grayscale-light);
}

.username {
    cursor: pointer;
}

.username:hover {
    text-decoration: underline;
}
</style>