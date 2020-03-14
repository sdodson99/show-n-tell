<template>
    <div id="image-container" class="d-flex align-items-center justify-content-center">
        <img id="explore-image" :style="{ maxHeight: maxHeight }" :src="imageUri"/>
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
    height: 100%;
}

#explore-image{
    max-width: 100%;
    background: white;
}
</style>