import Follow from './follow'

function User(email, username, dateJoined, following){
    this.email = email
    this.username = username
    this.dateJoined = dateJoined
    this.following = following
}

User.fromJSON = function(user) {
    return new User(
        user.email,
        user.username,
        user.dateJoined,
        user.following ? user.following.map(f => Follow.fromJSON(f)) : []
    )
}

export default User