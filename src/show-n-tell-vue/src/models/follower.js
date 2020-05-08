function Follower(followerEmail, followerUsername) {
    this.followerEmail = followerEmail
    this.followerUsername = followerUsername
}

Follower.fromJSON = function(follow) {
    return new Follower(
        follow.followerEmail,
        follow.followerUsername
    )
}

export default Follower