export const crud = {    
    selectedItems: null,            
    showDialog : false,
    deleteDialog : false,    
    loading: false,
    dialogTitle : '',
    totalRecords: 0,
    items: null,
    lazyParams: {},
    webService : null,
    serviceName : null,
    detailServiceName : null,
    detailsFKName : null,
    detailsRouteName : null,
    component : null,
    itemTitle : null,
    expandable : false,
    defaultCurrent : {},
    functions : {
        loadLazyData,
        onPage,
        onSort ,
        onFilter,
        openNew,
        save,
        edit,
        deleteCurrent,
        deleteSelected,        
        confirmDelete,
        confirmDeleteSelected,
        onExpand,
        onDataChanged : null  
    }
}

export default crud;

function loadLazyData(vars) {
    vars.loading = true;

    vars.webService.read(vars.serviceName, vars.lazyParams ).then(data => {
        vars.items = data.Data;
        vars.totalRecords = data.Count;
        vars.loading = false;
    });
}

function onExpand(vars, event) {  
    const params = {
        filters : {}
    };     
    params.filters[vars.detailsFKName] = {value: event.data.Id, matchMode : 'equals'};
    

    vars.webService.read(vars.detailsServiceName, params ).then(data => {        
        event.data.details = data.Data;        
        vars.loading = false;
    });
}

function onPage(vars, event) {   
    vars.lazyParams = event;
    vars.functions.loadLazyData(vars);
}

function onSort(vars, event) {    
    vars.lazyParams = event;
    vars.functions.loadLazyData(vars);
}

function onFilter(vars, event) {    
    vars.loading = true;
    vars.lazyParams.filters = vars.component.filters;
    vars.functions.loadLazyData(vars);
}
export function openNew(vars) {
    vars.component.current = vars.defaultCurrent,
    vars.dialogTitle = 'Add new item';            
    vars.showDialog = true;            
}

function save(vars, current) {     
    if (!vars.validate()) return;     
    const promise = current.Id ? vars.webService.update(vars.serviceName, current.Id, current)
                               : vars.webService.create(vars.serviceName, current);
    return handle(promise, vars, `${vars.itemTitle(current)} ${current.Id ? 'updated' : 'added'}`)
           .then(_ => {
            if (vars.functions.onDataChanged)
                vars.functions.onDataChanged();   
           });
}

function deleteCurrent(vars, item) {            
    const promise = vars.webService.delete(vars.serviceName, item.Id);
    handle(promise, vars, `${vars.itemTitle(item)} deleted`)
        .then(_ => {
            vars.deleteDialog = false;
            if (vars.functions.onDataChanged)
                    vars.functions.onDataChanged();   
        }); 
}
function deleteSelected(vars, selected) {
    let promises = [];
    selected.forEach(
        (item, index) => {
            const promise = vars.webService.delete(vars.serviceName, item.Id);
            promises.push(promise);
            handle(promise, vars, `${vars.itemTitle(item)} deleted`, true)
                .then(vars.deleteDialog = false); 
        }
    )                         
    Promise.allSettled(promises).then( _ => {     
        vars.deleteSelectedDialog = false;
        vars.functions.loadLazyData(vars);        
        vars.selectedItems = [];
        if (vars.functions.onDataChanged)
            vars.functions.onDataChanged();   
    });
           
}

function confirmDelete(vars, item) {                    
    vars.component.current = item;
    vars.deleteDialog = true;
}     
function confirmDeleteSelected(vars, selected) {                        
    vars.deleteSelectedDialog = true;
}   
function edit(vars, item) {                    
    vars.component.current = {...item};
    vars.dialogTitle = vars.itemTitle(vars.component.current);
    vars.showDialog = true;
}

function handle(promise, vars, text, multiple = false) {
    return promise.then(data => {                                                                                                                
                    vars.component.$toast.add({
                        severity:'success', 
                        summary: 'Successful', 
                        detail: text, 
                        life: 3000});
                    if (!multiple) {
                        vars.functions.loadLazyData(vars);
                        vars.showDialog = false;
                    }                           
                })
            .catch(error => {                                                               
                    const details = error.response.data;
                    vars.component.$toast.add({
                        severity:'error', 
                        summary: `${details.status} ${details.title}`, 
                        detail: details.detail 
                    });                                                        
                });   

}
