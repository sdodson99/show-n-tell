import UnauthorizedError from '../../errors/unauthorized-error'

class CommentVueService{
    constructor(commentService, authenticationService, router) {
        this.commentService = commentService
        this.authenticationService = authenticationService
        this.router = router
    }

    /**
     * Post a comment on an image post.
     * @param {ImagePost} imagePost The image post to comment on.
     * @param {string} comment The comment content to add to the image post.
     * @param {string} [authRedirect] A router login redirect url if authentication fails. 
     * @returns {Comment} The created comment. Null if comment creation failed.
     */
    async createComment(imagePost, comment, authRedirect) {
        let createdComment = null;

        if(comment) {
            if(!this.authenticationService.isLoggedIn()) {
                this.redirectToLogin(authRedirect)
            } else {
                try {
                    createdComment = await this.commentService.createComment(imagePost.id, comment)
                    createdComment.username = this.authenticationService.getUser().username
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin(authRedirect)
                    }
                }
            }
        }

        return createdComment
    }

    /**
     * Update a comment on an image post.
     * @param {ImagePost} imagePost The image post to update comments on.
     * @param {number} commentId The id of the comment to update.
     * @param {string} content The new comment content.
     * @param {string} [authRedirect] A router login redirect url if authentication fails. 
     * @returns {Comment} The updated comment. Null if the comment update failed.
     */
    async updateComment(imagePost, commentId, content, authRedirect) {
        let updatedComment = null;

        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin(authRedirect)
        } else {
            try {
                updatedComment = await this.commentService.updateComment(imagePost.id, commentId, content)
                updatedComment.username = this.authenticationService.getUser().username
            } catch (error) {
                if(error instanceof UnauthorizedError){
                    this.redirectToLogin(authRedirect)
                }
            }
        }

        return updatedComment
    }

    /**
     * Delete a comment on an image post.
     * @param {ImagePost} imagePost The image post to delete the comment from.
     * @param {number} commentId The id of the comment to delete.
     * @param {string} [authRedirect] A router login redirect url if authentication fails. 
     * @returns {Comment} The deleted image post comment. Null if the comment delete failed.
     */
    async deleteComment(imagePost, commentId, authRedirect) {
        let deletedComment = null;

        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin(authRedirect)
        } else {
            try {
                if(await this.commentService.deleteComment(imagePost.id, commentId)) {
                    deletedComment = imagePost.comments.find(c => c.id === commentId)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError){
                    this.redirectToLogin(authRedirect)
                }
            }
        }

        return deletedComment
    }

    /**
     * Redirect to the login screen.
     * @param {string} [authRedirect] A router login redirect url. 
     */
    redirectToLogin(authRedirect) {
        if(authRedirect) {
            this.router.push({path: "/login", query: { redirect: authRedirect }})
        } else {
            this.router.push({path: "/login", query: { back: true }})
        }
    }
}

export default CommentVueService