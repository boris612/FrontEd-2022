<template>
    <DataTable :value="crud.items" :lazy="true" :paginator="true" :rows="rows" :filters="filters" ref="dt"                    
        v-model:selection="crud.selectedItems"
        :totalRecords="crud.totalRecords" :loading="crud.loading" 
        @page="crud.functions.onPage(crud, $event)" @sort="crud.functions.onSort(crud, $event)">
        <Column selectionMode="multiple" headerStyle="width: 3rem" :exportable="false"></Column>
        <Column v-for="(col, index) in columns" :key="index" :field="col.field" :header="col.header" 
                filterMatchMode="startsWith"  :sortable="true">  
            <template #body="{data}" v-if="col.formatFunction">                
                {{col.formatFunction(data[col.field])}}
            </template>
            <template #filter>
                <InputText type="text" v-model="filters[col.field]['value']" 
                @keydown.enter="crud.functions.onFilter(crud, $event)" class="p-column-filter" 
                />
            </template>           
        </Column>                                                            
        <Column :exportable="false">  
            <template #header>
                <Button label="New" icon="pi pi-plus" class="p-button-success p-mr-2" @click="crud.functions.openNew(crud)" />
                <Button label="Delete" icon="pi pi-trash" class="p-button-danger" 
                        @click="crud.functions.confirmDeleteSelected(crud, crud.selectedItems)" 
                        :disabled="!crud.selectedItems || !crud.selectedItems.length" />
            </template>                      
            <template #body="slotProps">
                <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" 
                        @click="crud.functions.edit(crud, slotProps.data)" />
                <Button icon="pi pi-trash" class="p-button-rounded p-button-warning" 
                        @click="crud.functions.confirmDelete(crud, slotProps.data)" />               
            </template>
        </Column>               
    </DataTable>

    <delete-dialogs :crud="crud" />
    
</template>

<script>
import DeleteDialogs from '@/components/DeleteDialogs.vue'
 
export default {
    components: {
        DeleteDialogs        
    }, 
    props: ['filters', 'columns', 'crud', 'rows']
}
</script>