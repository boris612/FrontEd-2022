<template>
    <div class="layout-content">             
        <div class="content-section implementation">
            <div class="card">                                

                <my-table :columns="columns" :filters="filters" :crud="crud" :rows="rows"/>

                <Dialog v-model:visible="crud.showDialog" :style="{width: '450px'}" :header="crud.dialogTitle" :modal="true" class="p-fluid">                            
                    <div class="p-field">
                        <label for="postcode">Postcode</label>
                        <InputText id="postcode" v-model.trim="v$.current.Postcode.$model"  :class="{'p-invalid':v$.current.Postcode.$error}"/>
                        <div v-for="(error, index) in v$.current.Postcode.$errors" :key="index">
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

<script>
import WebService from '@/services/WebService';
import useVuelidate from '@vuelidate/core';
import {required, maxLength, integer} from '@vuelidate/validators';
import crud from '@/scripts/table-crud';
import MyTable from '@/components/Table.vue'
import NewItemFooter from '@/components/NewItemFooter.vue';
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
                'Postcode': {value: '', matchMode: 'equals'},                
            },           
            columns: [
                {field: 'Postcode', header: 'Postcode'},
                {field: 'Name', header: 'Name'},                
            ]
        }
    },    
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'towns';
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

        this.crud.functions.loadLazyData(this.crud);
    },
    validations() {
        return {
            current: {
                Name: {
                    required,
                    maxLength : maxLength(100)
                },
                Postcode: {
                    required, integer                    
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
            return `${item.Postcode} ${item.Name}`;
        }
    }
}
</script>

