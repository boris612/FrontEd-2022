<template>
    <div class="p-d-flex p-flex-column p-p-3">
        <div class="p-mb-2">
            <span class="p-input-icon-left">
                <i class="pi pi-user" />              
                <InputText type="text" placeholder="Username"
                    v-model.trim="v$.username.$model"
                    :class="{'p-invalid':v$.username.$error}"
                    @keyup.enter="login"
                 />
            </span>
        </div>
        <div class="p-mb-2">
            <span class="p-input-icon-left">
                <i class="pi pi-lock" />                
                <InputText type="password" placeholder="Password"
                    v-model.trim="v$.password.$model"
                    :class="{'p-invalid':v$.password.$error}"
                     @keyup.enter="login"
                 />
            </span>
        </div>
        <div class="p-mb-2">
           <Button label="Login" 
            :disabled="invalid"
            @click="login"
            class="p-mr-2 p-mb-2"/>
        </div>
    </div>   
</template>

<script>
import WebService from '@/services/WebService';
import useVuelidate from '@vuelidate/core';
import {required} from '@vuelidate/validators';

export default {    
    setup () {
        return { v$: useVuelidate()}
    }, 
    data() {
        return {                    
            submitted : false,
            username : '',
            password : '',
            loginService : null
        }
    },    
    created() {        
        this.loginService = new WebService();        
    },
    mounted() {        
        this.submitted = false;               
    },
    validations() {
        return {
            username: { required} ,                
            password : { required} 
        }
    },
    computed: {
        invalid() {           
            return this.submitted || !this.username || !this.password;
        }
    },
    methods: {
        validate() {
            this.v$.$touch();
            return !this.v$.$error;
        },
        login() {
            if (this.validate()) {
                this.submitted = true;                
                this.loginService.login(this.username, this.password)
                    .then(_ => {
                        this.submitted = false;                        
                        this.$router.replace({name: 'home'});                       
                    })                
                    .catch(error => {         
                            console.error(JSON.stringify(error));                
                            const details = error.response.data;                            
                            this.$toast.add({
                                severity:'error', 
                                summary: `${details.status} ${details.title}`, 
                                detail: details.detail 
                            });
                            this.submitted = false;
                        });   
                }
        }
    }
}
</script>

