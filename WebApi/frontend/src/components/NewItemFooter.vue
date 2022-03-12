<template>
    <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="crud.showDialog = false"/>
    <Button label="Save" icon="pi pi-check" class="p-button-text" @click="crud.functions.save(crud, crud.component.current)" />
</template>


<script>
export default {
    props: ['crud']
}
</script>