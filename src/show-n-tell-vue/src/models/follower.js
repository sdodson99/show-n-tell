function Follower(email, username) {
    this.email = email
    this.username = username
}

Follower.fromJSON = function(follow) {
    return new Follower(
        follow.email,
        follow.username
    )
}

export default Follower