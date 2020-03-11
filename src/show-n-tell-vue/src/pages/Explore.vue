<template>
  <div>
    <div class="d-flex flex-column flex-sm-row justify-content-between">
      <button class="m-1 order-sm-2" type="button">Next</button>
      <button class="m-1 order-sm-1" type="button">Previous</button>
    </div>
    <div class="my-3 text-center">
      <img id="explore-image" class="mw-100" src="https://images.pexels.com/photos/255379/pexels-photo-255379.jpeg"/>
    </div>
    <div class="my-1 text-center">
      <div>posted by {{ username }}</div>
      <div>{{ datePosted }}</div>
    </div>
    <div class="my-3 text-center">
      {{ description }}
    </div>
  </div>
</template>

<script>
export default {
  name: "Explore",
  props: {
    isLoggedIn: Boolean
  },
  data: function(){
    return {
      username: "dodsonsean",
      datePosted: new Date().toDateString(),
      description: "hello world"
    }
  }
};
</script>

<style scoped>
  #explore-image{
    border: 1px solid var(--color-primary-dark);
    border-radius: 3px;
  }

  button{
    border: none;
    padding: 1em;
    background: var(--color-secondary-medium);
    color: var(--color-primary-dark);
    border-radius: 5px;
    min-width: 100px;
  }
</style>
