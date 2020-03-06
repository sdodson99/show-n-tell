const apiUrl = "https://localhost:44323/api"

//Elements
const btnSubmit = document.getElementById("file-submit")
const btnGet = document.getElementById("file-get")
const inputFile = document.getElementById("file-upload")
const imgOutput = document.getElementById("image")
const statusContent = document.getElementById("status")

//Track the last uploaded image id
let lastUploadedId

//Post the attached file to the server when submit button clicked.
btnSubmit.addEventListener("click", async (e) => {
	let status = await postFile(inputFile.files[0])

	switch (status) {
		case 200:
			statusContent.innerText = "Successfully uploaded image."
			break
		case 400:
			statusContent.innerText = "Please provide an image."
			break
		case 401:
			statusContent.innerText = "You must login to upload an image."
			break
		case 403:
			statusContent.innerText = "You do not have permission to upload an image."
			break
		default:
			statusContent.innerText = "Unable to upload image."
	}
})

//Get an image from the server and display it as the img source.
btnGet.addEventListener("click", async (e) => {
	if (lastUploadedId) {
		try {
			const image = await getImage(lastUploadedId)
			imgOutput.removeAttribute("src")
			imgOutput.setAttribute("src", image.imageUri)
			statusContent.innerText = ""
		} catch (err) {
			statusContent.innerText = "Image does not exist."
		}
	} else {
		statusContent.innerText = "Please upload an image first."
	}
})

//Handle errors if an image that does noe exist is shown.
imgOutput.onerror = (e) => {
	statusContent.innerText = "Image does not exist."
	imgOutput.removeAttribute("src")
}

//Post file to the API.
//Returns true/false for success.
async function postFile(file) {

	//Add the file to the form.
	const formData = new FormData()
	formData.append('file', file)

	//Make the post request.
	const result = await fetch(`${apiUrl}/posts`, {
		method: 'POST',
		headers: {
			'Authorization': 'bearer ' + localStorage.getItem("accessToken")
		},
		body: formData
	})

	if (result.ok) {
		const createdPost = await result.json();
		lastUploadedId = createdPost.id
	}

	return result.status
}

//Get image data from the server for a specific id.
async function getImage(id) {

	//Make the GET request.
	const result = await fetch(`${apiUrl}/posts/${id}`, {
		headers: {
			'Content-Type': 'application/json',
			'Authorization': 'bearer ' + localStorage.getItem("accessToken")
		}
	})

	//Deserialize to JSON.
	return await result.json();
}