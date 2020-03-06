import { html, PolymerElement } from "@polymer/polymer/polymer-element.js";

class CreateImageForm extends PolymerElement {
  static get template() {
    return html`
            <style>
                :host {
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                }
                
                input[type=file] {
                    margin: 0.5em;
                    padding: 0.25em;
                    border: 1px solid #343F4B;
                    border-radius: 5px;
                }
                
                button {
                    margin: 0.5em;
                    padding: 5px 10px;
                    border-radius: 5px;
                    background: #343F4B;
                    color: #FFD185;
                    text-decoration: none;
                    font-size: 1.5em;
                    cursor: pointer;
                }
            </style>

            <input id="file-upload" type="file"/>
            <button id="file-submit" type="button" onclick=[[submitImage]]>Submit</button>
            <button id="file-get" type="button" onclick=[[getImage]]>Get Image</button>
        `;
  }

  static get properties() {
    return {
      files: {
        type: FileList
      }
    };
  }

  submitImage() {
    const submitImageClicked = new CustomEvent('submit-image-clicked', {
      detail: {
        image: this.getFileInput()
      },
      bubbles: true,
      composed: true
    });
    this.dispatchEvent(submitImageClicked);
  }

  getImage() {
    const getImageClicked = new CustomEvent('get-image-clicked', {
      bubbles: true,
      composed: true
    });
    this.dispatchEvent(getImageClicked);
  }

  getFileInput() {
    return this.root.querySelector("#file-upload").files[0] || null;
  }

  constructor() {
    super();
    this.submitImage = this.submitImage.bind(this);
    this.getImage = this.getImage.bind(this);
  }

}

window.customElements.define('create-image-form', CreateImageForm);