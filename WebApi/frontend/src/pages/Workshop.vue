<template>
    <div class="layout-content">              
        <div class="content-section implementation">
            <div class="card">   
                <div class="p-fluid p-formgrid p-grid p-mt-3">
                        <div class="p-field p-col">
                        <label for="title">Title</label>
                        <InputText id="title" type="text" v-model="v$.workshop.Title.$model"
                        :class="{'p-invalid':v$.workshop.Title.$error}" />
                         <div v-for="(error, index) in v$.workshop.Title.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div> 
                    </div>
                    <div class="p-field p-col">
                        <label for="description">Description</label>
                        <InputText id="description" type="text" v-model="workshop.Description" />                        
                    </div>
                    <div class="p-field p-col">
                        <label for="capacity">Capacity</label>
                        <InputText id="capacity" type="text" v-model="v$.workshop.Capacity.$model" 
                        :class="{'p-invalid':v$.workshop.Capacity.$error}"
                        />
                         <div v-for="(error, index) in v$.workshop.Capacity.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div> 
                    </div>
                     <div class="p-field p-col">
                        <label for="freeplaces">Free places</label>
                        <InputText id="freeplaces" readonly type="text" v-model="workshop.FreePlaces" />                          
                    </div>
                    <div class="p-field p-col">
                        <label for="time">Time</label>
                        <Calendar id="time" v-model="v$.workshop.Time.$model"  
                            dateFormat="yy-mm-dd" :showTime="true" 
                            :class="{'p-invalid':v$.workshop.Time.$error}"/>
                        <div v-for="(error, index) in v$.workshop.Time.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>  
                    </div>
                    <div class="p-field p-col">
                        <label for="school">School</label>
                         <Dropdown id="school" v-model="v$.workshop.SchoolId.$model" :options="schools" 
                                  :class="{'p-invalid':v$.workshop.SchoolId.$error}"
                                  optionValue="Value" optionLabel="Text" :filter="true" 
                                  placeholder="Select a school" :showClear="true" />   
                        <div v-for="(error, index) in v$.workshop.SchoolId.$errors" :key="index">
                            <small class="p-error">{{ error.$message }}</small>
                        </div>                                                  
                    </div>
                    
                </div>
                <div class="p-field p-col">
                    <Button label="Save" @click="saveWorkshop" />
                </div>
                  

                <my-table :columns="columns" :filters="filters" :crud="crud" :rows="rows"/>            
               
                <Dialog v-model:visible="crud.showDialog" :header="crud.dialogTitle" :modal="true" class="p-fluid">                                               
                     <div class="p-field">
                        <label for="student">Student</label>
                        <Dropdown id="student" v-model="v$.current.StudentId.$model" :options="students" 
                                  :class="{'p-invalid':v$.current.StudentId.$error}"
                                  optionValue="Value" optionLabel="Text" :filter="true" 
                                  placeholder="Select a student" :showClear="true" />   
                        <div v-for="(error, index) in v$.current.StudentId.$errors" :key="index">
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
import {required, maxLength, minValue, helpers, integer} from '@vuelidate/validators';
import {crud, openNew} from '@/scripts/table-crud';
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
            workshop : {},
            filters: {
                'WorkshopId' : {value : this.$route.params.id, matchMode : 'equals'},
                'Name': {value: '', matchMode: 'contains'},
                'Surname': {value: '', matchMode: 'contains'},
                'Email': {value: '', matchMode: 'contains'},
                'StudentSchool': {value: '', matchMode: 'contains'}                           
            },            
            columns: [
                {field: 'Name', header: 'Name'},
                {field: 'Surname', header: 'Surname'},
                {field: 'Email', header: 'E-mail'},
                {field: 'StudentSchool', header: 'Participant school'}
            ],
            students : [],
            schools : []
        }
    },   
    created() {
        this.crud.component = this;
        this.crud.webService = new WebService();
        this.crud.serviceName = 'WorkshopParticipants';
        this.crud.itemTitle = this.itemTitle;
        this.crud.validate = this.validate;   
        this.crud.defaultCurrent = { WorkshopId : this.$route.params.id };  
        this.crud.functions.openNew =  (vars) => {
            this.loadStudents();
            openNew(vars);
        }  
        this.crud.functions.onDataChanged =  _ => this.loadWorkshop();        
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

        this.loadWorkshop();


        this.crud.functions.loadLazyData(this.crud);
    },
    validations() {
        return {
            current: {               
                StudentId : {
                    required : helpers.withMessage("You have to choose a student", required)
                }
            },
            workshop: {
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
            this.v$.current.$touch();            
            return !this.v$.current.$error;
        },
        validateWorkshop() {
            this.v$.workshop.$touch();            
            return !this.v$.workshop.$error;
        },
        itemTitle(item) {
            if (item.Name)
                return `${item.Name} ${item.Surname}`;
            else
                return 'Participant';
        },
        loadStudents() {
            this.crud.webService.lookup('students', {workshopId : this.$route.params.id}).then(data => {
                this.students = data;                                        
            });
        },
        loadWorkshop() {
            this.crud.webService.read('workshops', {filters : 
                                        {Id : {value : this.$route.params.id, matchMode : 'equals'}}})
                .then(data => {                
                    this.workshop = data.Data[0];                                        
            });
        },
        saveWorkshop(event) {            ;            
            if (!this.validateWorkshop()) return;     
            const promise = this.crud.webService.update('workshops', this.workshop.Id, this.workshop);                                    
            this.handle(promise);
        },
        handle(promise) {
            return promise.then(data => {                                                                                                                
                            this.$toast.add({
                                severity:'success', 
                                summary: 'Successful', 
                                detail: 'Workshop data updated', 
                                life: 3000});
                            this.loadWorkshop();
                        })
                    .catch(error => {                                                               
                            const details = error.response.data;
                            this.$toast.add({
                                severity:'error', 
                                summary: `${details.status} ${details.title}`, 
                                detail: details.detail 
                            });                                                        
                        });   
        }
    }
}
</script>

