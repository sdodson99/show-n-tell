import UnauthorizedError from '../../errors/unauthorized-error'

class LikeVueService{
    constructor(likeService, router, currentUser) {
        this.likeService = likeService
        this.router = router
        this.currentUser = currentUser
    }

    /**
     * Like an image post and redirect to login if unauthorized.
     * @param {ImagePost} imagePost 
     */
    async likeImagePost(imagePost) {
        if(this.currentUser && !this.isUsersPost(imagePost)) {
            try {
                const like = await this.likeService.likeImagePost(imagePost.id)

                imagePost.likes.push(like)
            } catch (error) {
                if(error instanceof UnauthorizedError){
                    this.router.push({path: "/login"})
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
        if(this.currentUser && !this.isUsersPost(imagePost, this.currentUser)) {
            try {
                if(await this.likeService.unlikeImagePost(imagePost.id)){
                    imagePost.likes = imagePost.likes.filter(l => l.userEmail !== this.currentUser.email)
                }
            } catch (error) {
                if(error instanceof UnauthorizedError){
                    this.$router.push({path: "/login"})
                }
            }
        }

        return imagePost.likes
    }

    isUsersPost(imagePost) {
        return imagePost.email === this.currentUser.email
    }
}

export default LikeVueService