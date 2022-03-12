<template>
    <Dialog v-model:visible="crud.deleteDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
        <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="crud.component.current">Are you sure you want to delete <b>{{crud.itemTitle(crud.component.current)}}</b>?</span>
        </div>
        <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="crud.deleteDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="crud.functions.deleteCurrent(crud, crud.component.current)" />
        </template>
    </Dialog>

     <Dialog v-model:visible="crud.deleteSelectedDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
        <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span>Are you sure you want to delete the selected items?</span>
        </div>
        <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="crud.deleteSelectedDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="crud.functions.deleteSelected(crud, crud.selectedItems)" />
        </template>
    </Dialog>
</template>

<script>
export default {
    props: ['crud']
}
</script>