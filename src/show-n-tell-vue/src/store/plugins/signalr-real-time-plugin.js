const signalr = require("@microsoft/signalr");

import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../modules/image-posts/types'
import { ModuleName as FeedModuleName, Action as FeedAction } from '../modules/feed/types'

import ImagePost from '../../models/image-post'
import Like from '../../models/like';
import Comment from '../../models/comment';

export default function createRealTimePlugin(hubUrl){
    return (store) => {
        const connection = new signalr.HubConnectionBuilder().withUrl(hubUrl).build()

        connection.on("IMAGE_POST_CREATED", data => {
            const imagePost = ImagePost.fromJSON(data);

            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])

            const currentUser = store.state.user.currentUser;

            // If current user is following image post creator, add image post to feed.
            if(currentUser.following.some(f => f.email === imagePost.userEmail)) {
                store.dispatch(`${FeedModuleName}/${FeedAction.ADD_NEW_IMAGE_POST_ID}`, imagePost.id)
            }
        })

        connection.on("IMAGE_POST_UPDATED", data => {
            const imagePost = ImagePost.fromJSON(data)
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])
        })

        connection.on("IMAGE_POST_DELETED", id => {
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_IMAGE_POST}`, id)
            store.dispatch(`${FeedModuleName}/${FeedAction.REMOVE_IMAGE_POST_ID}`, id)
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
            
        })

        connection.on("PROFILE_UNFOLLOW", data => {
            
        })

        connection.start().then(() => console.log("Connected to real-time hub."))
    }
}