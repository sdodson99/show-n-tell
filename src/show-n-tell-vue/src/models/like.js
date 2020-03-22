function Like(userEmail, dateCreated) {
    this.userEmail = userEmail
    this.dateCreated = new Date(dateCreated)
}

Like.fromJSON = function(like) {
    return new Like(
        like.userEmail,
        like.dateCreated
    )
}

export default Like