<template>
  <li class="px-3 py-2 my-dropdown-item"
    @click="() => $emit('click')">
      <slot></slot>
  </li>
</template>

<script>
export default {
    name: "MoreDropdownItem"
}
</script>

<style scoped>

.my-dropdown-item {
    white-space: nowrap;
    cursor: pointer;
}

.my-dropdown-item:hover {
    background: var(--color-grayscale-light);
}
</style>