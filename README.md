# About

The repository contains the source code of the example shown during a presentation at the FrontEdWorkshop at the Faculty of Electrical Engineering and Computer Science in March 2022.

The purpose of the example is to provide an overview of creating a web service in . NET, focusing on creating a typical CRUD WebAPI, and describing a GraphQL service as a possible workaround for the problem of overfetching and underfetching.

The example with a WebAPI is sort of a predecessor to the presentation from the previous year (https://github.com/boris612/FrontedWorkshop-WebApiDemo), which had used command-query separation with generic handlers for Entity Framework.

# Running the example

## The first steps
1. Create the database using FrontEd.sql to create a database. The script is designed for Microsoft SQL Server. 
2. Modify appsettings.json, appsettings.Development.json, or create a secret file associated with UserSecredIt set to FrontEd to define the connection string. This applies to both WebApi and GraphQL demo

## WebAPI

Start the backend from the Visual Studio or from the command line. Look at what port it is running on. For example, if it is running at port 44393 (as set in Properties/launchSettings.json), https://localhost:44393/docs shoud provide the Swagger documentation.

Go to fronted/settings.js and set apiUrl to https://localhost:44393 (or the appropriate port)
The port for the frontend part is defined in vite.config.js (currently 3001)

Restore dependencies using `yarn install` and run with `yarn run dev` that would serve the client the http://localhost:3001/

You can log in with the username _rade_, and password _admin_ created with the previous run script.

## GraphQL
Run the GraphQLWebService, go to  https://localhost:44340/graphql, and try the examples from Examples/queries.md
