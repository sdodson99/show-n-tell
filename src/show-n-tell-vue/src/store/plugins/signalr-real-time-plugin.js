const signalr = require("@microsoft/signalr");

import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../modules/image-posts/types'
import { ModuleName as FeedModuleName, Mutation as FeedMutation } from '../modules/feed/types'
import { ModuleName as ProfileModuleName, Mutation as ProfileMutation } from '../modules/profile/types'
import { ModuleName as ExploreModuleName, Action as ExploreAction } from '../modules/explore/types'

import ImagePost from '../../models/image-post'
import Like from '../../models/like';
import Comment from '../../models/comment';
import Follower from '../../models/follower';

export default function createRealTimePlugin(hubUrl){
    return (store) => {
        const connection = new signalr.HubConnectionBuilder().withUrl(hubUrl).build()

        connection.on("IMAGE_POST_CREATED", data => {
            const imagePost = ImagePost.fromJSON(data);

            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])

            const currentUser = store.state.user.currentUser;

            if(currentUser.following.some(f => f.email === imagePost.userEmail)) {
                store.commit(`${FeedModuleName}/${FeedMutation.ADD_IMAGE_POST_ID_TO_BEGINNING}`, imagePost.id)
            }

            const currentProfileUsername = store.state.profile.profileUsername
            const imagePostUsername = imagePost.username
            
            if(currentProfileUsername === imagePostUsername) {
                store.commit(`${ProfileModuleName}/${ProfileMutation.ADD_IMAGE_POST_ID_TO_BEGINNING}`, imagePost.id)
            }
        })

        connection.on("IMAGE_POST_UPDATED", data => {
            const imagePost = ImagePost.fromJSON(data)
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])
        })

        connection.on("IMAGE_POST_DELETED", id => {
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_IMAGE_POST}`, id)
            store.commit(`${FeedModuleName}/${FeedMutation.REMOVE_IMAGE_POST_ID}`, id)
            store.commit(`${ProfileModuleName}/${ProfileMutation.REMOVE_IMAGE_POST_ID}`, id)
            store.dispatch(`${ExploreModuleName}/${ExploreAction.REMOVE_IMAGE_POST_BY_ID}`, id)
        })

        connection.on("IMAGE_POST_LIKE", data => {
            const imagePostId = data.imagePostId
            const like = Like.fromJSON(data)

            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.ADD_LIKE_TO_IMAGE_POST}`, { imagePostId, like })
        })

        connection.on("IMAGE_POST_UNLIKE", data => {
            const imagePostId = data.imagePostId
            const like = Like.fromJSON(data)

            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_LIKE_FROM_IMAGE_POST}`, { imagePostId, like})
        })

        connection.on("IMAGE_POST_COMMENT_CREATED", data => {
            const imagePostId = data.imagePostId
            const comment = Comment.fromJSON(data)
            
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.ADD_COMMENT_TO_IMAGE_POST}`, { imagePostId, comment })
        })

        connection.on("IMAGE_POST_COMMENT_UPDATED", data => {
            const imagePostId = data.imagePostId
            const comment = Comment.fromJSON(data)
            
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_COMMENT_ON_IMAGE_POST}`, { imagePostId, comment })
        })

        connection.on("IMAGE_POST_COMMENT_DELETED", data => {
            const imagePostId = data.imagePostId
            const commentId = data.commentId

            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_COMMENT_FROM_IMAGE_POST}`, { imagePostId, commentId })
        })

        connection.on("PROFILE_FOLLOW", data => {
            const followedProfileUsername = data.userUsername
            const currentProfileUsername = store.state.profile.profileUsername

            if(followedProfileUsername === currentProfileUsername) {
                const follower = new Follower(data.followerEmail, data.followerUsername)
                store.commit(`${ProfileModuleName}/${ProfileMutation.ADD_FOLLOWER_TO_PROFILE}`, follower)
            }
        })

        connection.on("PROFILE_UNFOLLOW", data => {
            const unfollowedProfileUsername = data.userUsername
            const currentProfileUsername = store.state.profile.profileUsername

            if(unfollowedProfileUsername === currentProfileUsername) {
                const followerEmail = data.followerEmail
                store.commit(`${ProfileModuleName}/${ProfileMutation.REMOVE_FOLLOWER_FROM_PROFILE}`, followerEmail)
            }
        })

        connection.start().then(() => console.log("Connected to real-time hub."))
    }
}