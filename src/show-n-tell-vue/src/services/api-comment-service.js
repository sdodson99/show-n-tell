import Comment from '../models/comment'

class APICommentService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Post a comment on an image post.
     * @param {Number} id The id of the image post to comment on.
     * @param {String} content The content of the new comment.
     */
    async createComment(id, content) {
        const url = `${this.baseUrl}/imageposts/${id}/comments`

        const commentRequest = {
            content: content
        }

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(commentRequest)
        });        

        const commentResponse = await apiResponse.json();

        return Comment.fromJSON(commentResponse)
    }
}

export default APICommentService