function Follow(userEmail, userUsername, followerEmail, followerUsername) {
    this.userEmail = userEmail
    this.userUsername = userUsername
    this.followerEmail = followerEmail
    this.followerUsername = followerUsername
}

Follow.fromJSON = function(follow) {
    return new Follow(
        follow.userEmail,
        follow.userUsername,
        follow.followerEmail,
        follow.followerUsername
    )
}

export default Follow