<template>
    <div v-if="$slots.default">
        <img id="more" @click="toggleOpen" src="../../assets/icons/more.png"/>
        <div id="my-dropdown" v-if="isOpenData">
            <div id="my-dropdown-content">
                <ul id="my-dropdown-list">
                    <slot id="content" class="d-flex"></slot>
                </ul>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "MoreDropdown",
    props: {
        isOpen: Boolean
    },
    data: function() {
        return {
            isOpenData: this.isOpen
        }
    },
    methods: {
        toggleOpen: function() {
            this.isOpenData = !this.isOpenData
        },
        close: function() {
            this.isOpenData = false
        }
    }
}
</script>

<style scoped>
#my-dropdown-list{
    border: 1px solid var(--color-primary-dark);
    list-style: none;
    border-radius: 3px;
    background: white;
    min-width: 125px;
}

#more {
    cursor: pointer;
}

#my-dropdown {
    position: absolute;
}

#my-dropdown-content {
    position: relative;
    z-index: 1000;
    top: 0;
}
</style>