<template>
    <div>
        <img id="more" @click="toggleOpen" src="../../assets/icons/more.png"/>
        <div id="dropdown" v-if="isOpen">
            <div id="dropdown-content">
                <slot id="content" class="d-flex"></slot>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "MoreDropdown",
    data: function() {
        return {
            isOpen: false
        }
    },
    methods: {
        toggleOpen: function() {
            this.isOpen = !this.isOpen
        }
    }
}
</script>

<style scoped>
#more {
    cursor: pointer;
}

#dropdown {
    position: absolute;
}

#dropdown-content {
    position: relative;
    z-index: 1000;
    top: 0;
}
</style>