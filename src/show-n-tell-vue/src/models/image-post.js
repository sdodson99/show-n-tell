import User from './user'
import Like from './like'
import Comment from './comment'

function ImagePost(id, userEmail, imageUri, description, dateCreated, user, likes, comments) {
    this.id = id
    this.userEmail = userEmail
    this.imageUri = imageUri
    this.description = description
    this.dateCreated = new Date(dateCreated)

    this.user = User.fromJSON(user)
    this.likes = likes.map(l => Like.fromJSON(l))
    this.comments = comments.map(c => Comment.fromJSON(c))

    this.username = this.user.username
}

ImagePost.fromJSON = function(imagePost) {
    return new ImagePost(
        imagePost.id, 
        imagePost.userEmail, 
        imagePost.imageUri, 
        imagePost.description, 
        imagePost.dateCreated, 
        imagePost.user || {}, 
        imagePost.likes || [], 
        imagePost.comments || [])
}

export default ImagePost