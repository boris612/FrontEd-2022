import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import interceptors from './scripts/interceptors.js'

import "primeflex/primeflex.css";
import "primevue/resources/themes/saga-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";

import PrimeVue from 'primevue/config';
import Dialog from 'primevue/dialog';
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import InputText from "primevue/inputtext";
import Toast from "primevue/toast";
import ToastService from "primevue/toastservice";
import Toolbar from "primevue/toolbar";
import Dropdown from "primevue/dropdown"
import Menubar from "primevue/menubar"
import Password from "primevue/password"
import Checkbox from "primevue/checkbox"
import Calendar from "primevue/calendar"


const app = createApp(App);
app.use(router);

app.config.globalProperties.window = window;

app.use(PrimeVue, { ripple: true });
app.use(ToastService);

app.component('Dialog', Dialog);
app.component("Button", Button);
app.component("DataTable", DataTable);
app.component("Column", Column);
app.component("InputText", InputText);
app.component("Toast", Toast);
app.component("Toolbar", Toolbar);
app.component("Dropdown", Dropdown);
app.component("Menubar", Menubar);
app.component("Password", Password);
app.component("Checkbox", Checkbox);
app.component("Calendar", Calendar);

app.mount('#app');


