const apiUrl = "https://localhost:44323/api";

class ImagePostService {
  // Get image data from the server for a specific id.
  // Return the image data.
  async getById(id) {
    // Make the GET request.
    const result = await fetch(`${apiUrl}/posts/${id}`, {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + localStorage.getItem("accessToken")
      }
    }); // Deserialize to JSON.

    return await result.json();
  } // Post image to the API.
  // Returns request status code.


  async create(image) {
    // Add the image to the form data for the request.
    const formData = new FormData();
    formData.append('file', image); // Make the post request.

    const result = await fetch(`${apiUrl}/posts`, {
      method: 'POST',
      headers: {
        'Authorization': 'bearer ' + localStorage.getItem("accessToken")
      },
      body: formData
    });

    if (result.ok) {
      const createdPost = await result.json();
      this.lastUploadedId = createdPost.id;
    }

    return result.status;
  }

}

export default ImagePostService;