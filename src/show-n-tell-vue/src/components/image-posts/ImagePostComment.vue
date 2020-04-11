<template>
    <div>
        <div class="d-flex flex-column flex-sm-row">
            <div class="username font-weight-bold"
                @click="() => $emit('usernameClicked', username)">
                {{ username }}
            </div>
            <div class="mx-3 d-none d-sm-block">|</div>
            <div>
                {{ dateCreated.toLocaleDateString() }}
            </div>
        </div>
        <div class="mt-2">
            {{ content }}
        </div>
        <div v-if="!content" class="mt-2 font-weight-light">
            <i>{{ fallbackContent }}</i>
        </div>
    </div>
</template>

<script>
export default {
    name: "ImagePostComment",
    props: {
        username: String,
        content: String,
        fallbackContent: String,
        dateCreated: Date
    }
}
</script>

<style scoped>

.username {
    cursor: pointer;
}

.username:hover {
    text-decoration: underline;
}

</style>