<template>
    <div class="layout-content">              
        <div class="content-section implementation">
            <div class="card">
                <my-table :columns="columns" :filters="filters" :crud="crud" :rows="rows"/>

                <Dialog v-model:visible="crud.showDialog" :header="crud.dialogTitle" :modal="true" class="p-fluid">                                               
                     <div class="p-field">
                        <label for="town">Town</label>
                        <Dropdown v-model="v$.current.TownId.$model" :options="towns" 
                                  :class="{'p-invalid':v$.current.TownId.$error}"
                                  optionValue="Value" optionLabel="Text" :filter="true" 
                                  placeholder="Select a Town" :showClear="true" />   
                        <div v-for="(error, index) in v$.current.TownId.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                                                  
                    </div>
                    <div class="p-field">
                        <label for="name">Name</label>
                        <InputText id="name" v-model.trim="v$.current.Name.$model"  :class="{'p-invalid':v$.current.Name.$error}"/>
                        <div v-for="(error, index) in v$.current.Name.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                     <template #footer>
                        <new-item-footer :crud="crud" />                        
                    </template>
                </Dialog>                                
            </div>
		</div>
    </div>
</template>

<style scoped>

.p-dialog-content {
    height: 600px;
}
</style>

<script>
import WebService from '@/services/WebService';
import useVuelidate from '@vuelidate/core';
import {required, maxLength, helpers} from '@vuelidate/validators';
import crud from '@/scripts/table-crud';
import MyTable from '@/components/Table.vue'
import NewItemFooter from '@/components/NewItemFooter.vue'
import settings from '../settings';
 
export default {
    components: {
        MyTable,
        NewItemFooter,
    },
    setup () {
        return { v$: useVuelidate()}
    }, 
    data() {
        return {
            crud : {...crud},    
            rows : settings.pageSize,                   
            current : {},
            filters: {
                'Name': {value: '', matchMode: 'contains'},                
                'Town': {value: '', matchMode: 'contains'},                
            },           
            columns: [
                {field: 'Name', header: 'Name'},
                {field: 'Town', header: 'Town'},                
            ],
            towns : []
        }
    },    
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'schools';
        this.crud.itemTitle = this.itemTitle;
        this.crud.validate = this.validate;       
    },
    mounted() {
        this.crud.loading = true;
        
        this.crud.lazyParams = {
            first: 0,
            rows: this.rows,
            sortField: null,
            sortOrder: null,
            filters: this.filters
        };

        this.crud.webService.lookup('towns').then(data => {
                    this.towns = data;                                        
                });
        this.crud.functions.loadLazyData(this.crud);
    },
    validations() {
        return {
            current: {
                Name: {
                    required,
                    maxLength : maxLength(100)
                },
                TownId : {
                    required : helpers.withMessage("You have to choose a town", required)
                }
            }
        }
    },
    methods: {
        validate() {
            this.v$.$touch();
            return !this.v$.$error;
        },
        itemTitle(item) {
            return `${item.Name} ${item.Town}`;
        }
    }
}
</script>

