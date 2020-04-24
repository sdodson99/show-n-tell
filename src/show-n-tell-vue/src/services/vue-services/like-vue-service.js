import UnauthorizedError from '../../errors/unauthorized-error'

/**
 * Vue service to like image posts.
 */
class LikeVueService{
    constructor(likeService, authenticationService, router) {
        this.likeService = likeService
        this.authenticationService = authenticationService
        this.router = router
    }

    /**
     * Like an image post and redirect to login if unauthorized.
     * @param {ImagePost} imagePost The image post to like.
     * @param {string} [authRedirect] A router login redirect url if authentication fails. 
     * @returns {Like} The new like for the image post. Null if the like failed.
     */
    async likeImagePost(imagePost, authRedirect) {
        let like = null;

        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin(authRedirect)
        } else {
            const currentUser = this.authenticationService.getUser()

            if(!this.isUsersPost(imagePost, currentUser)) {
                try {
                    like = await this.likeService.likeImagePost(imagePost.id)
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin(authRedirect)
                    }          
                } 
            }
        } 

        return like
    }

    /**
     * Unike an image post and redirect to login if unauthorized.
     * @param {ImagePost} imagePost The image post to unlike.
     * @param {string} [authRedirect] A router login redirect url if authentication fails. 
     * @returns {Like} The like removed from the image post. Null if the unlike failed.
     */
    async unlikeImagePost(imagePost, authRedirect) {
        let unlike = null

        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin(authRedirect)
        } else {
            const currentUser = this.authenticationService.getUser()

            if(!this.isUsersPost(imagePost, currentUser)) {
                try {
                    if(await this.likeService.unlikeImagePost(imagePost.id)){
                        unlike = imagePost.likes.find(l => l.userEmail === currentUser.email)
                    }
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin(authRedirect)
                    }
                }
            }
        } 

        return unlike
    }

    /**
     * Check if a user owns an image post.
     * @param {ImagePost} imagePost The image post to check.
     * @param {User} currentUser The current user.
     * @returns {boolean} True/false for success.
     */
    isUsersPost(imagePost, currentUser) {
        return imagePost.email === currentUser.email
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

export default LikeVueService