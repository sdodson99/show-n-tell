<template>
    <svg @click="clicked" viewBox="0 0 30 30">
        <path class="icon" :style="style" d="M4 7h22M4 15h22M4 23h22"></path>
    </svg>
</template>

<script>
export default {
    name: "Hamburger",
    props: {
        toggled: {
            type: Boolean,
            default: false
        },
        toggleOnStyle: {
            type: String,
            default: "stroke: gray"
        },
        toggleOffStyle: {
            type: String,
            default: "stroke: black"
        }
    },
    data: function(){
        return {
            toggleState: this.toggled
        }
    },
    computed: {
        style: function() {
            return this.toggleState ? this.toggleOnStyle : this.toggleOffStyle;
        }
    },
    methods: {
        clicked: function() {
            this.toggleState = !this.toggleState
            this.$emit(this.toggleState ? 'toggleOn' : 'toggleOff')
        }
    }
}
</script>

<style>
.icon{
    stroke-width: 2;
    stroke-linecap: round;
    stroke-miterlimit: 10;
}
</style>