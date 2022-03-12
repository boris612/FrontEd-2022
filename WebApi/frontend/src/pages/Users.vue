<template>
    <div class="layout-content">             
        <div class="content-section implementation">
            <div class="card">                                

                <my-table :columns="columns" :filters="filters" :crud="crud" :rows="rows"/>

                <Dialog v-model:visible="crud.showDialog" :style="{width: '450px'}" :header="crud.dialogTitle" :modal="true" class="p-fluid">                            
                    <div class="p-field">
                        <label for="username">User Name</label>
                        <InputText id="username" v-model.trim="v$.current.UserName.$model"  :class="{'p-invalid':v$.current.UserName.$error}"/>
                        <div v-for="(error, index) in v$.current.UserName.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                    <div class="p-field">
                        <label for="firstname">First Name</label>
                        <InputText id="firstname" v-model.trim="v$.current.FirstName.$model"  :class="{'p-invalid':v$.current.FirstName.$error}"/>
                        <div v-for="(error, index) in v$.current.FirstName.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                    <div class="p-field">
                        <label for="lastname">Last Name</label>
                        <InputText id="lastname" v-model.trim="v$.current.LastName.$model"  :class="{'p-invalid':v$.current.LastName.$error}"/>
                        <div v-for="(error, index) in v$.current.LastName.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>                    
                    <div class="p-field" v-if="current.Id">
                        Change password
                        <Checkbox v-model="current.ChangePassword" :binary="true" />
                    </div>                   
                    <div class="p-field" v-if="!current.Id || current.ChangePassword === true">
                        <label for="password">Password</label>
                        <span class="p-input-icon-left">
                            <i class="pi pi-lock" />                
                            <InputText id="password" type="password" placeholder="Password"
                                v-model.trim="v$.current.Password.$model"
                                :class="{'p-invalid':v$.current.Password.$error}"                                
                            />
                        </span>
                        <div v-for="(error, index) in v$.current.Password.$errors" :key="index">
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
import {required, maxLength, requiredIf } from '@vuelidate/validators';
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
                'UserName': {value: '', matchMode: 'contains'},
                'FirstName': {value: '', matchMode: 'contains'},
                'LastName': {value: '', matchMode: 'contains'},
            },           
            columns: [
                {field: 'UserName', header: 'User Name'},
                {field: 'FirstName', header: 'First Name'},                
                {field: 'LastName', header: 'Last Name'},
            ]
        }
    },    
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'users';
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
                UserName: {
                    required,
                    maxLength : maxLength(100)
                },
                FirstName: {
                    required,
                    maxLength : maxLength(100)
                },
                LastName: {
                    required,
                    maxLength : maxLength(100)
                },
                Password : {
                     //required : requiredIf(this.passwordRequired) //does not work as expected!
                }                
            }
        }
    },
    methods: {
        validate() {               
            if (this.passwordRequired() && !this.current.Password)
                return false;        
            this.v$.$touch();
            return !this.v$.$error;
        },
        itemTitle(item) {
            return `${item.UserName} (${item.FirstName} ${item.LastName})`;
        },
        passwordRequired() {                      
            return !this.current.Id || this.current.ChangePassword === true;
        }
    }
}
</script>

