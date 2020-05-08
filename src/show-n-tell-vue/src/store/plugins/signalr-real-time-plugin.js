const signalr = require("@microsoft/signalr");

import { ModuleName as ImagePostsModuleName, Mutation as ImagePostsMutation } from '../modules/image-posts/types'

export default function createRealTimePlugin(hubUrl){
    return (store) => {
        const connection = new signalr.HubConnectionBuilder().withUrl(hubUrl).build()

        connection.on("IMAGE_POST_CREATED", imagePost => {
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])

        })

        connection.on("IMAGE_POST_UPDATED", imagePost => {
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.UPDATE_IMAGE_POSTS}`, [imagePost])
        })

        connection.on("IMAGE_POST_DELETED", id => {
            store.commit(`${ImagePostsModuleName}/${ImagePostsMutation.REMOVE_IMAGE_POST}`, id)
        })

        connection.start().then(() => console.log("Connected to real-time hub."))
    }
}