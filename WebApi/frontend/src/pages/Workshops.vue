<template>
    <div class="layout-content">              
        <div class="content-section implementation">
            <div class="card">
                <master-detail-table :columns="columns" 
                    :detailsColumns="detailsColumns"
                    :filters="filters" :crud="crud" :rows="rows"/>

                <Dialog v-model:visible="crud.showDialog" :header="crud.dialogTitle" :modal="true" class="p-fluid">                                               
                     <div class="p-field">
                        <label for="school">School</label>
                        <Dropdown id="school" v-model="v$.current.SchoolId.$model" :options="schools" 
                                  :class="{'p-invalid':v$.current.SchoolId.$error}"
                                  optionValue="Value" optionLabel="Text" :filter="true" 
                                  placeholder="Select a school" :showClear="true" />   
                        <div v-for="(error, index) in v$.current.SchoolId.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                                                  
                    </div>
                    <div class="p-field">
                        <label for="title">Title</label>
                        <InputText id="title" v-model.trim="v$.current.Title.$model"  :class="{'p-invalid':v$.current.Title.$error}"/>
                        <div v-for="(error, index) in v$.current.Title.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                    <div class="p-field">
                        <label for="description">Description</label>
                        <InputText id="description" v-model.trim="current.Description"/>                                            
                    </div>
                    <div class="p-field">
                        <label for="time">Time</label>
                        <Calendar id="time" v-model="v$.current.Time.$model"  
                            dateFormat="yy-mm-dd" :showTime="true" 
                            :class="{'p-invalid':v$.current.Time.$error}"/>
                        <div v-for="(error, index) in v$.current.Time.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                        
                    </div>
                    <div class="p-field">
                        <label for="capacity">Capacity</label>
                        <InputText id="capacity" v-model.trim="v$.current.Capacity.$model"  :class="{'p-invalid':v$.current.Capacity.$error}"/>
                        <div v-for="(error, index) in v$.current.Capacity.$errors" :key="index">
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
import {required, maxLength, helpers, integer, minValue} from '@vuelidate/validators';
import crud from '@/scripts/table-crud';
import NewItemFooter from '@/components/NewItemFooter.vue'
import MasterDetailTable from '@/components/MasterDetailTable.vue';
import settings from '../settings';
 
export default {
    components: {        
        NewItemFooter,
        MasterDetailTable,
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
                'Title': {value: '', matchMode: 'contains'},                
                'Description': {value: '', matchMode: 'contains'}, 
                'School': {value: '', matchMode: 'contains'},
                'Time': {value: '', matchMode: 'contains'},
                'Capacity': {value: '', matchMode: 'equals'},
                'FreePlaces': {value: '', matchMode: 'equals'},               
            },           
            columns: [
                {field: 'Title', header: 'Title'},               
                {field: 'Description', header: 'Description'},                
                {field: 'Time', header: 'Time', formatFunction : this.formatDate},
                {field: 'Capacity', header: 'Capacity'},
                {field: 'FreePlaces', header: 'Free places'},
                {field: 'School', header: 'School'},                
            ],
            detailsColumns: [
                {field: 'Name', header: 'Name'},
                {field: 'Surname', header: 'Surname'},
                {field: 'Email', header: 'E-mail'},                
            ],
            schools : []
        }
    },    
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'workshops';
        this.crud.detailsServiceName = 'WorkshopParticipants';
        this.crud.detailsRouteName = 'workshop-details';
        this.crud.detailsFKName = 'WorkshopId';
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
                Title: {
                    required,
                    maxLength : maxLength(100)
                },
                Time: {
                    required                    
                },
                Capacity: {
                    required,
                    integer,
                    minValue : minValue(1)
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
            return `${item.Title} ${item.School}`;
        },
        formatDate(value) {
            return value; //TO DO: format date and time
        },
    }
}
</script>

