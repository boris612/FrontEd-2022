Open https://localhost:44340/graphql/, and create new tab
## Get all towns (cannot be paged)
```
{
  allTowns {
    postcode, name
  }
}
```
It creates a request with the following body
```
{
  "query": "{\n  allTowns {\n    postcode\n    name\n  }\n}\n"
}
```

## Get all towns (except Zagreb) that has schools 
```
{
  allTowns (
    where: {
      name: { ncontains : "Zagreb"} 
      schools : {any : true}    
    }
    order: {name:ASC}
  ) {
    postcode, name,
    schools {
      name
    }
  }
}
```

## Get all towns that has 'gimnazija'
```
{
  allTowns (
    where: {
      name: { ncontains : "Zagreb"} 
      schools : {
        any : true
        some: {name : {contains : "gimnazija"}}
      }    
    }
    order: {name:ASC}
  ) {
    postcode, name,
    schools {
      name
    }
  }
}
```

## Get first 5 students with their school name, and town order by student's last name
```
{
  students (first: 5, order: {surname : ASC}) {
    totalCount,
    pageInfo {
      hasNextPage, hasPreviousPage,
      startCursor, endCursor
    }
    nodes {
      name, surname,
      school {
        name 
        town {
          name
        }
      }
    }
  }
}
```
and the next five
```
{
  students (first: 5,after : "NA==", order: {surname : ASC}) {
    totalCount,
    pageInfo {
      hasNextPage, hasPreviousPage,
      startCursor, endCursor
    }
    nodes {
      name, surname,
      school {
        name 
        town {
          name
        }
      }
    }
  }
}
```

## Get all workshops and participants emails

```
{
  workshops {
    totalCount    
        
    nodes {      
      title,
      capacity
      workshopParticipants  {           
        participant  {
          email
        }
      }
    }
  }
}
```

## participant of a particular workshop
```
{
  participants(workshopId:1) {
    participant {
      name, surname, email
    }
  }
}
```

## Add a town
```
mutation {
  addTown(input : {
    name : "Town name",
    postcode : 123
  }) {
    id
  }
}
```

## update a town
```
mutation {
  updateTown(id: 47, input : {
    name : "Some new name",
    postcode : 123
  }) {
    name
  }
}
```

## delete a town (e.g. with id 47)
```
mutation {
  deleteTown(id: 47)
}
```
