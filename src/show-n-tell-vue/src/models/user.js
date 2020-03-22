function User(email, username, dateJoined){
    this.email = email
    this.username = username
    this.dateJoined = dateJoined
}

User.fromJSON = function(user) {
    return new User(
        user.email,
        user.username,
        user.dateJoined
    )
}

export default User