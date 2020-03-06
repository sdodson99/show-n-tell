import { html, PolymerElement } from "../../../node_modules/@polymer/polymer/polymer-element.js";
import ImagePostService from "../../services/image-post-service.js";
import "../../../node_modules/@google-web-components/google-signin/google-signin.js";
import "../create-image-form/create-image-form.js";

class ShowNTellApp extends PolymerElement {
  static get template() {
    return html`
      <style>
        * {
          margin: 0;
          padding: 0;
          box-sizing: border-box;
        }

        :host {
          display: flex;
          flex: 1;
          flex-direction: column;
        }

        #nav {
          font-weight: normal;
          font-size: 3em;
          text-align: center;
          background: #343F4B;
          color: #FFD185;
          padding: 0.25em;
        }

        #content {
          display: flex;
          flex-direction: column;
          align-items: center;
        }

        .message {
          color: #343F4B;
          font-size: 1.5em;
          margin-top: 0.5em;
        }

        google-signin{
          margin: 1em;
        }

        #image {
          margin-top: 1em;
          max-width: 100%;
        }
      </style>

      <h1 id="nav">Show 'N Tell Prototype</h1>
      <div id="content">
        <p class="message">[[statusMessage]]</p>

        <google-signin client-id="462162693296-pesibunbs71up7lress4c6b549bd2qld.apps.googleusercontent.com"
            signed-in={{isSignedIn}}
            on-signed-in-changed="signIn"></google-signin>

        <create-image-form on-submit-image-clicked="createImage"
          on-get-image-clicked="showImage"></create-image-form>

        <p class="message">[[imageDescription]]</p>
        <img id="image" src=[[imageUri]] onerror="this.style.display='none'" onload="this.style.display=''"/> 
      </div>
    `;
  }

  static get properties() {
    return {
      isSignedIn: {
        type: Boolean,
        value: 'false'
      },
      statusMessage: {
        type: String,
        value: ''
      },
      imageDescription: {
        type: String,
        value: ''
      },
      imageUri: {
        type: String
      }
    };
  }

  signIn() {
    if (typeof gapi !== 'undefined') {
      if (!this.isSignedIn) {
        // Log user in if not signed in already.
        const accessToken = gapi.auth2.getAuthInstance().currentUser.get().getAuthResponse().id_token;
        window.localStorage.setItem("accessToken", accessToken);
      } else {
        // Log user out if user is signed in.
        window.localStorage.clear("accessToken");
      }
    }
  } // Post the attached file to the server when submit event raised.


  async createImage(e) {
    let uploadedImage = e.detail.image;
    let status = await this.imagePostService.create(uploadedImage);

    switch (status) {
      case 200:
        this.statusMessage = "Successfully uploaded image.";
        break;

      case 400:
        this.statusMessage = "Please provide an image.";
        break;

      case 401:
        this.statusMessage = "You must login to upload an image.";
        break;

      case 403:
        this.statusMessage = "You do not have permission to upload an image.";
        break;

      default:
        this.statusMessage = "Unable to upload image.";
    }
  }

  async showImage() {
    let lastUploadedId = this.imagePostService.lastUploadedId;

    if (lastUploadedId) {
      try {
        const image = await this.imagePostService.getById(lastUploadedId);
        this.imageDescription = `Description: "${image.description}"`;
        this.imageUri = image.imageUri;
        this.statusMessage = "";
      } catch (err) {
        this.statusMessage = "Image does not exist.";
      }
    } else {
      this.statusMessage = "Please upload an image first.";
    }
  }

  constructor() {
    super();
    this.imagePostService = new ImagePostService();
    this.signIn = this.signIn.bind(this);
    this.createImage = this.createImage.bind(this);
  }

}

window.customElements.define('show-n-tell-app', ShowNTellApp);