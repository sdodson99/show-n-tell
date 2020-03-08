<template>
  <div>
    {{ message }}
    <button v-on:click="updateMessage">Test</button>
  </div>
</template>

<script>

function updateMessage(){
  this.message = "test"
}

export default {
  name: "Home",
  data: function() {
    return {
      message: "hello"
    }
  },
  methods: {
    updateMessage
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped></style>
