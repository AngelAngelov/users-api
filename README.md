# users-api

Simple API that exposes functionality to create, read, update and delete (CRUD) users.

User model: 

 - **id** - a unique user id

 - **email** - a user's email address

 - **givenName** - in the UK this is the user's first name

 - **familyName** - in the UK this is the user's last name

 - **created** - the date and time the user was added

Project pre requirements: 
 - .Net core 2.2 (installation guide [Linux](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-2.2.103))
 
 
Application is uploaded live at Microsoft Azure

URL: https://users-testapp.azurewebsites.net/

## Project setup 
 
 Clone the repo 
 
 ```
 git clone 
 ```
 
 Run: 
 
 ```
 dotnet restore
 ```
 
 Change directory to /Users.Api
 
 ```
 cd Users.Api
 ```
 
 Run:
 
 ```
 dotnet run
 ```
 
 Application should be started at localhost, port 5000
 
 ```
 http://localhost:5000
 ```
 
 ## End points 
 
 The application is split on 2 parts: 
  1. #### Public end point #####
  
   **Route:** /graphql
  
   **Usage:** This is a graphql end point for retrieving queries with public data. The data suppose to be public and the application allows CORS without any restrictions (just for sample). Two queries can be performed: 
  
   **GET ALL USERS**
   
    ```
    {
      users {
        id, 
        email, 
        givvenName, 
        familyName
      }
    }
    ```
  
   **GET USER BY ID**
   
   ```
    {
      user(id: "00000000-0000-0000-0000-000000000000") {
        email, 
        givvenName, 
        familyName
      }
    }
   ```
  
  2. #### Not-public endpoints ####
  
     - **Route:** /api/user/{id}

       **Type:** GET

       **Data type:** json

       **Description:** Get speciffic user by id.
        
     - **Route:** /api/user/all

       **Type:** GET

       **Data type:** json

       **Description:** Get all users
        
     - **Route:** /api/user/

       **Type:** POST

       **Data type:** json

       **Description:** Create new user. Required fields email, givvenName, familyName
        
     - **Route:** /api/user/

       **Type:** PUT

       **Data type:** json

       **Description:** Edit user. Required fields id, email, givvenName, familyName
        
     - **Route:** /api/user/{id}

       **Type:** DELETE

       **Data type:** json

       **Description:** Remove speciffic user by id.
      
   ## Integration tests
   
   To run the integration tests you have to go to test project directory /Users.Api.IntegrationTest
   
   Run: 
   
   ```
   dotnet test
   ```
   
   Tests' results sould be displayed in the console.
