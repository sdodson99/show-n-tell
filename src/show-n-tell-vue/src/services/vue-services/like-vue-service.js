import UnauthorizedError from '../../errors/unauthorized-error'

class LikeVueService{
    constructor(likeService, authenticationService, router) {
        this.likeService = likeService
        this.authenticationService = authenticationService
        this.router = router
    }

    /**
     * Like an image post and redirect to login if unauthorized.
     * @param {ImagePost} imagePost 
     */
    async likeImagePost(imagePost) {
        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin()
        } else {
            const currentUser = this.authenticationService.getUser()

            if(!this.isUsersPost(imagePost, currentUser)) {
                try {
                    const like = await this.likeService.likeImagePost(imagePost.id)
                    imagePost.likes.push(like)
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin()
                    }          
                } 
            }
        } 

        return imagePost.likes
    }

    /**
     * Unike an image post and redirect to login if unauthorized.
     * @param {ImagePost} imagePost 
     */
    async unlikeImagePost(imagePost) {
        if(!this.authenticationService.isLoggedIn()) {
            this.redirectToLogin()
        } else {
            const currentUser = this.authenticationService.getUser()

            if(!this.isUsersPost(imagePost, currentUser)) {
                try {
                    if(await this.likeService.unlikeImagePost(imagePost.id)){
                        imagePost.likes = imagePost.likes.filter(l => l.userEmail !== currentUser.email)
                    }
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.redirectToLogin()
                    }
                }
            }
        } 

        return imagePost.likes
    }

    isUsersPost(imagePost, currentUser) {
        return imagePost.email === currentUser.email
    }

    redirectToLogin() {
        this.router.push({path: "/login", query: { back: true }})
    }
}

export default LikeVueService