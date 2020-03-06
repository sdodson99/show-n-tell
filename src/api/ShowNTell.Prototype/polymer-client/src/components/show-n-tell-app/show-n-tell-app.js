import {html, PolymerElement} from '@polymer/polymer/polymer-element.js';

import '@google-web-components/google-signin';

class ShowNTellApp extends PolymerElement {
  static get template() {
    return html`
      <style>
        :host {
          display: block;
        }
      </style>
      <google-signin client-id="462162693296-pesibunbs71up7lress4c6b549bd2qld.apps.googleusercontent.com"
          signed-in={{isSignedIn}}
          on-signed-in-changed="signIn"></google-signin>
    `;
  }

  static get properties() {
    return {
      isSignedIn: {
        type: Boolean,
        value: 'false'
      }
    };
  }

  signIn() {
    if(typeof gapi !== 'undefined'){
      if(!this.isSignedIn){
        // Log user in if not signed in already.
        const accessToken = gapi.auth2.getAuthInstance().currentUser.get().getAuthResponse().id_token
        window.localStorage.setItem("accessToken", accessToken)
      } else {
        // Log user out if user is signed in.
        window.localStorage.clear("accessToken")
      }
    }
  }

  constructor() {
    super()

    this.signIn = this.signIn.bind(this)
  }
}

window.customElements.define('show-n-tell-app', ShowNTellApp);
