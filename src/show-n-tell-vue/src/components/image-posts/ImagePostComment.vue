<template>
    <div>
        <div class="d-flex flex-column flex-sm-row align-items-center justify-content-between">
            <div class="d-flex flex-column flex-sm-row">
                <div class="username font-weight-bold"
                    @click="() => $emit('username-clicked', username)">
                    {{ username }}
                </div>
                <div class="mx-3 d-none d-sm-block">|</div>
                <div>
                    {{ dateCreated.toLocaleDateString() }}
                </div>
            </div>
            <more-dropdown ref="dropdown">
                <more-dropdown-item
                    v-if="canEdit"
                    @click="editComment">Edit</more-dropdown-item>
                <more-dropdown-item
                    v-if="canDelete"
                    @click="deleteComment">{{ isDeleting ? "Deleting..." : "Delete" }}</more-dropdown-item>
            </more-dropdown>
        </div>
        <div class="mt-2">
            <div v-if="!isEditing">
                {{ contentData }}
            </div>
            <div class="text-right" v-else>
                <textarea class="form-control" placeholder="New comment..." 
                    v-model="editContent"></textarea>
                <button type="button" class="my-3" 
                    :disabled="!validComment"
                    @click="submitEditComment">Submit</button>
            </div>
        </div>
        <div v-if="!content" class="mt-2 font-weight-light">
            <i>{{ fallbackContent }}</i>
        </div>
    </div>
</template>

<script>
import MoreDropdown from '../utilities/MoreDropdown'
import MoreDropdownItem from '../utilities/MoreDropdownItem'

export default {
    name: "ImagePostComment",
    components: {
        MoreDropdown,
        MoreDropdownItem
    },
    props: {
        username: String,
        content: String,
        dateCreated: Date,
        canDelete: Boolean,
        canEdit: Boolean,
        fallbackContent: String
    },
    data: function() {
        return {
            isDeleting: false,
            isEditing: false,
            contentData: this.content,
            editContent: this.content
        }
    },
    watch: {
        content: function() {
            this.contentData = this.content
            console.log('test');
            
        }
    },
    computed: {
        validComment: function() {
            return this.editContent
        }
    },
    methods: {
        editComment: function() {
            this.isEditing = true;
            this.$refs.dropdown.close()
        },
        deleteComment: function() {
            this.isDeleting = true;
            this.$emit('deleted')
        },
        submitEditComment: function() {
            this.$emit('edited', this.editContent)
            this.contentData = this.editContent
            this.isEditing = false;
        }
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