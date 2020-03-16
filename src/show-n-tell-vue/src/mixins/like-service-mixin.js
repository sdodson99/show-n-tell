import UnauthorizedError from '../errors/unauthorized-error'

export default {
    methods: {
        _likeImage: async function(imagePost) {
            if(!this._isUsersPost(imagePost, this.currentUser)) {
                try {
                    const like = await this.likeService.likeImagePost(imagePost.id)
                    imagePost.likes.push(like)
                } catch (error) {
                    if(error instanceof UnauthorizedError){
                        this.$router.push({path: "/login"})
                    }          
                } 
            }

            return imagePost.likes
        },
        _unlikeImage: async function(imagePost) {
            if(!this._isUsersPost(imagePost, this.currentUser)) {
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
        },
        _isUsersPost: function(imagePost, currentUser) {
            return imagePost.userEmail === (currentUser && currentUser.email)
        }
    }
}