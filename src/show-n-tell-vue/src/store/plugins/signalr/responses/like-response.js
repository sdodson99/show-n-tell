function LikeResponse(imagePostId, userEmail, username, dateCreated) {
    this.imagePostId = imagePostId
    this.userEmail = userEmail
    this.username = username
    this.dateCreated = new Date(dateCreated)
}

LikeResponse.fromJSON = function(like) {
    return new LikeResponse(
        like.imagePostId,
        like.userEmail,
        like.username,
        like.dateCreated
    )
}

export default LikeResponse