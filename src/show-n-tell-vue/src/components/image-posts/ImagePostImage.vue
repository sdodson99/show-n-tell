<template>
    <div id="image-container" @click="$emit('click')" class="d-flex align-items-center justify-content-center">
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
            this.$el.innerHTML = ""

            LoadImage(this.imageUri, (loadedImage) => {
                EXIF.getData(loadedImage, () => {
                    let orientation = EXIF.getTag(loadedImage, "Orientation");
                    
                    LoadImage(loadedImage.src, (orientedImage) => {
                        orientedImage.style.maxHeight = this.maxHeight
                        orientedImage.style.maxWidth = "100%"

                        this.$el.appendChild(orientedImage)
                    }, {
                        orientation: orientation
                    })
                })
            })  
        }
    }
}
</script>

<style scoped>
#image-container{
    background: var(--color-grayscale-light);
    border-radius: 3px;
    border: 1px solid var(--color-primary-dark);
    height: 100%;
}
</style>