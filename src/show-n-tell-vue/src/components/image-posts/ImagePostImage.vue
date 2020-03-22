<template>
    <div id="root">
        <div id="image-container" ref="imageContainer" @click="$emit('click')" class="d-flex align-items-center justify-content-center">
        </div>
    </div>
</template>

<script>
import LoadImage from 'blueimp-load-image'
import EXIF from 'exif-js'

export default {
    name: "ImagePostImage",
    props: {
        imageUri: String,
        maxHeight: {
            type: String,
            default: "25vh"
        }
    },
    mounted: function() {
        this.loadImage()
    },
    watch: {
        imageUri: function() {
            this.loadImage()
        }
    },
    methods: {
        loadImage: function() {
            this.clearImage()

            LoadImage(this.imageUri, (loadedImage) => {
                
                if(this.isJPEG(loadedImage.src)) {
                    EXIF.getData(loadedImage, () => {
                        let orientation = EXIF.getTag(loadedImage, "Orientation");
                        
                        LoadImage(loadedImage.src, (orientedImage) => {
                            this.setImage(orientedImage)
                        }, {
                            orientation: orientation
                        })
                    })
                } else {
                    this.setImage(loadedImage)
                }

            })  
        },
        setImage: function(image) {
            image.style.maxHeight = this.maxHeight
            image.style.maxWidth = "100%"

            this.$refs.imageContainer.appendChild(image)
        },
        clearImage: function() {
            this.$refs.imageContainer.innerHTML = ""
        },        
        isJPEG: function(src) {
            return src.endsWith('jpeg') || src.endsWith('jpg')
        }
    }
}
</script>

<style scoped>
#root {
    height: 100%;
    min-height: 100px;
}

#image-container{
    background: var(--color-grayscale-light);
    border-radius: 3px;
    border: 1px solid var(--color-primary-dark);
    min-height: 100px;
    height: 100%;
}
</style>