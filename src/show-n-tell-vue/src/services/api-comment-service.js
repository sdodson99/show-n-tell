import Comment from '../models/comment'

/**
 * Service to add comments to image posts.
 */
class APICommentService{
    constructor(baseUrl, apiClient) {
        this.baseUrl = baseUrl
        this.apiClient = apiClient
    }

    /**
     * Post a comment on an image post.
     * @param {number} id The id of the image post to comment on.
     * @param {string} content The content of the new comment.
     * @returns {Comment} The created comment.
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

    /**
     * Update the content of a comment.
     * @param {number} imagePostId The id of the image post with the comment.
     * @param {number} commentId The id of the comment.
     * @param {string} content The content for the comment to update.
     */
    async updateComment(imagePostId, commentId, content) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}/comments/${commentId}`

        const commentRequest = {
            content: content
        }

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(commentRequest)
        });

        const commentResponse = await apiResponse.json();

        return Comment.fromJSON(commentResponse)
    }

    /**
     * Delete a comment.
     * @param {number} imagePostId The id of the image post with the comment.
     * @param {number} commentId The id of the comment.
     */
    async deleteComment(imagePostId, commentId) {
        const url = `${this.baseUrl}/imageposts/${imagePostId}/comments/${commentId}`

        const apiResponse = await this.apiClient.authFetch(url, {
            method: "DELETE",
        });      

        return apiResponse.ok
    }
}

export default APICommentService