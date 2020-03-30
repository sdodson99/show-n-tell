import User from './user'
import Like from './like'
import Comment from './comment'

function ImagePost(id, userEmail, imageUri, description, dateCreated, tags, user, likes, comments) {
    this.id = id
    this.userEmail = userEmail
    this.imageUri = imageUri
    this.description = description
    this.dateCreated = new Date(dateCreated)

    this.tags = tags
    this.user = user
    this.likes = likes
    this.comments = comments

    this.username = this.user.username
}

ImagePost.fromJSON = function(imagePost) {
    return new ImagePost(
        imagePost.id, 
        imagePost.userEmail, 
        imagePost.imageUri, 
        imagePost.description, 
        imagePost.dateCreated, 
        imagePost.tags ? imagePost.tags.map(t => t.content) : [],
        imagePost.user ? User.fromJSON(imagePost.user) : {}, 
        imagePost.likes ? imagePost.likes.map(l => Like.fromJSON(l)) : [], 
        imagePost.comments ? imagePost.comments.map(c => Comment.fromJSON(c)) : [])
}

export default ImagePost