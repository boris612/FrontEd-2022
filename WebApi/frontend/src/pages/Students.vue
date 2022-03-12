<template>
    <div class="layout-content">              
        <div class="content-section implementation">
            <div class="card">    
                <my-table :columns="columns" :filters="filters" :crud="crud" :rows="rows"/>            
               
                <Dialog v-model:visible="crud.showDialog" :header="crud.dialogTitle" :modal="true" class="p-fluid">                                               
                     <div class="p-field">
                        <label for="school">Town</label>
                        <Dropdown v-model="v$.current.SchoolId.$model" :options="schools" 
                                  :class="{'p-invalid':v$.current.SchoolId.$error}"
                                  optionValue="Value" optionLabel="Text" :filter="true" 
                                  placeholder="Select a School" :showClear="true" />   
                        <div v-for="(error, index) in v$.current.SchoolId.$errors" :key="index">
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
                    <div class="p-field">
                        <label for="surnname">Surname</label>
                        <InputText id="surnname" v-model.trim="v$.current.Surname.$model"  :class="{'p-invalid':v$.current.Surname.$error}"/>
                        <div v-for="(error, index) in v$.current.Surname.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                    <div class="p-field">
                        <label for="email">E-mail</label>
                        <InputText id="email" v-model.trim="v$.current.Email.$model"  :class="{'p-invalid':v$.current.Email.$error}"/>
                        <div v-for="(error, index) in v$.current.Email.$errors" :key="index">
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

<style>

.p-dialog-content {
    height: 600px;
}
</style>

<script>
import WebService from '@/services/WebService';
import useVuelidate from '@vuelidate/core';
import {required, maxLength, helpers, email} from '@vuelidate/validators';
import crud from '@/scripts/table-crud';
import MyTable from '@/components/Table.vue';
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
                'Surname': {value: '', matchMode: 'contains'},
                'Email': {value: '', matchMode: 'contains'},
                'School': {value: '', matchMode: 'contains'},
                'Town': {value: '', matchMode: 'contains'},                
            },            
            columns: [
                {field: 'Name', header: 'Name'},
                {field: 'Surname', header: 'Surname'},
                {field: 'Email', header: 'Email'},
                {field: 'School', header: 'School'},
                {field: 'Town', header: 'Town'},                
            ],
            schools : []
        }
    },   
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'students';
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

       this.crud.webService.lookup('schools').then(data => {
                    this.schools = data;                                        
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
                Surname: {
                    required,
                    maxLength : maxLength(100)
                },
                Email: {
                    required,
                    email,
                    maxLength : maxLength(100)
                },
                SchoolId : {
                    required : helpers.withMessage("You have to choose a school", required)
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
            return `${item.Name} ${item.Surname})`;
        }
    }
}
</script>

