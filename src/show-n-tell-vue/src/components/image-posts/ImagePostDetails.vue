<template>
    <div>
        <div>
            posted by <a @click="viewProfile">{{ username }}</a>
        </div>
        <div>{{ formattedDateCreated }}</div>
    </div>
</template>

<script>
export default {
    name: "ImagePostDetails",
    props: {
        username: String,
        dateCreated: Date
    },
    computed: {
        formattedDateCreated: function() {
            return this.dateCreated.toLocaleDateString()
        }
    },
    methods: {
        viewProfile: function() {
            this.$router.push({path: `/profile/${this.username}`})
        }
    }
}
</script>

<style scoped>
    a, a:hover{
        color: unset;
        cursor: pointer;
    }

    a {
        font-weight: bold;
    }

    a:hover {
        text-decoration: underline;
    }
</style>