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
         <div class="text-center mb-3"
            v-else>
            You must be logged in to comment.
         </div>
         <ul v-if="hasComments">
             <li class="comment-item pt-2 pb-4"
                v-for="comment in comments"
                :key="comment.id">
                <image-post-comment
                    :content="comment.content"
                    :username="comment.username"
                    :canEdit="canEdit(comment)"
                    :canDelete="canDelete(comment)"
                    :dateCreated="comment.dateCreated"
                    @usernameClicked="(username) => $emit('usernameClicked', username)"
                    @deleted="() => $emit('deleted', comment.id)"
                    @edited="(content) => $emit('edited', {id: comment.id, content: content})"/>
             </li>
         </ul>
         <div class="mt-2 text-center" 
            v-else>
             No comments have been posted.
         </div>
    </div>
</template>

<script>
import ImagePostComment from './ImagePostComment'

export default {
    name: "ImagePostCommentList",
    components: {
        ImagePostComment
    },
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
        },
        currentUser: Object,
        imagePostUserEmail: String
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
        canEdit: function(comment) {
            return this.currentUser && comment.userEmail === this.currentUser.email
        },
        canDelete: function(comment) {
            return this.currentUser && (this.canEdit(comment) || this.imagePostUserEmail === this.currentUser.email)
        },
        submit: function() {
            if(this.validComment) {
                this.$emit('commented', this.newCommentContent)
                this.newCommentContent = ""
            }
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
</style>