function Following(email, username) {
    this.email = email
    this.username = username
}

Following.fromJSON = function(follow) {
    return new Following(
        follow.email,
        follow.username
    )
}

export default Following