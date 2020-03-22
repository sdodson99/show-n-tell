function Comment(id, userEmail, username, content, dateCreated){
    this.id = id
    this.userEmail = userEmail
    this.username = username
    this.content = content
    this.dateCreated = new Date(dateCreated)
}

Comment.fromJSON = function(comment) {
    return new Comment(
        comment.id,
        comment.userEmail,
        comment.username,
        comment.content,
        comment.dateCreated
    )
}

export default Comment