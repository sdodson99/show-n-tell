<template>
    <div id="root">
        <div id="image-container" ref="imageContainer" @click="$emit('click')" class="d-flex align-items-center justify-content-center">
            <img :src="imageUri" :style="{ maxHeight: maxHeight }"/>
        </div>
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