<template>
    <div>
    </div>
</template>

<script>
export default {
    name: "Search",
    props: {
        query: String,
        currentUser: Object
    },
    watch: {
        query: async function() {
            console.log(this.query);    
        }
    }
}
</script>

<style>

</style>