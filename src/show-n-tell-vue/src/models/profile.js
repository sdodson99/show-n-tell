import User from "./user"
import ImagePost from "./image-post"
import Follow from "./follow"

function Profile(email, username, dateJoined, imagePosts, followers, following) {
    this.email = email
    this.username = username
    this.dateJoined = new Date(dateJoined)
    this.imagePosts = imagePosts
    this.followers = followers
    this.following = following
}

Profile.fromJSON = function(profile) {
    return new Profile(
        profile.email,
        profile.username,
        profile.dateJoined,
        profile.imagePosts ? profile.imagePosts.map(p => ImagePost.fromJSON(p)) : [],
        profile.followers ? profile.followers.map(f => Follow.fromJSON(f)) : [],
        profile.following ? profile.following.map(f => Follow.fromJSON(f)) : []
    )
}

export default Profile