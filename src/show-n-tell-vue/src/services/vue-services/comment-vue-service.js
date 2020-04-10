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
     * @param {Comment} comment The comment to add to the image post.
     */
    async createComment(imagePost, comment, authRedirect) {
        if(comment) {
            if(!this.authenticationService.isLoggedIn()) {
                this.redirectToLogin(authRedirect)
            } else {
                try {
                    const createdComment = await this.commentService.createComment(imagePost.id, comment)
                    createdComment.username = this.authenticationService.getUser().username
        
                    imagePost.comments.push(createdComment)
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin(authRedirect)
                    }
                }
            }
        }

        return imagePost.comments
    }

    redirectToLogin(authRedirect) {
        if(authRedirect) {
            this.router.push({path: "/login", query: { redirect: authRedirect }})
        } else {
            this.router.push({path: "/login", query: { back: true }})
        }
    }
}

export default CommentVueService