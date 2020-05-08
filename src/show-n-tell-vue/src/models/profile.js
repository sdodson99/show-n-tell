import ImagePost from "./image-post"
import Follower from "./follower"
import Following from "./following"

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
        profile.followers ? profile.followers.map(f => Follower.fromJSON(f)) : [],
        profile.following ? profile.following.map(f => Following.fromJSON(f)) : []
    )
}

export default Profile