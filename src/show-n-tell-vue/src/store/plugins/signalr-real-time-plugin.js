const signalr = require("@microsoft/signalr");

import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../modules/image-posts/types'
import { ModuleName as FeedModuleName, Action as FeedAction } from '../modules/feed/types'

import ImagePost from '../../models/image-post'

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

        connection.start().then(() => console.log("Connected to real-time hub."))
    }
}