<template>
    <div id="image-container" ref="imageContainer" @click="$emit('click')" class="d-flex align-items-center justify-content-center">
        <img :src="imageUri" :style="{ maxHeight: maxHeight }"/>
    </div>
</template>

<script>
export default {
    name: "ImagePostImage",
    props: {
        imageUri: String,
        maxHeight: {
            type: String,
            default: "25vh"
        }
    }
}
</script>

<style scoped>

#image-container{
    background: var(--color-grayscale-light);
    border-radius: 3px;
    border: 1px solid var(--color-primary-dark);
    min-height: 100px;
    height: 100%;
}

img {
    max-width: 100%;
    max-height: 100%;
    image-orientation: from-image;
}
</style>