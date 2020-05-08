function Following(userEmail, userUsername) {
    this.userEmail = userEmail
    this.userUsername = userUsername
}

Following.fromJSON = function(follow) {
    return new Following(
        follow.userEmail,
        follow.userUsername
    )
}

export default Following