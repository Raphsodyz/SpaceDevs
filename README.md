If you like the project or gonna use for something else please leave a star! ✨

## Overview

This project is a Rest Web Api for the Space Launch List of the https://ll.thespacedevs.com/2.1.0/swagger. It has a complete CRUD, fuzzy text search and his own database that's feed from llapi. This API will not have a limitations like spacedevs API that blocks after 15 requests. The data is stored with you.

## Technologies
- .Net 7.0
- Entity Framework
- Dapper
- AutoMapper
- Asp.NetCore WebApi
- Nginx
- Postgresql
- GIST Indexes
- Docker
- GitHub Actions for CI
- XUnit
- Mock
- OpenApi (Swagger)

## Requirements
- Docker

## Execution
To run the project you only need to have Docker.
Download the code above. Extract the file. Inside the root of the extracted project (where the dockerfile and docker-compose.yml are) run the command in the shell or command prompt:

``` docker compose up -d ```

The container is initially configured the port 7000 (nginx). The API is available and the documentation can be viewed in the project by going to /swagger. (http://localhost:7000/swagger).

Note: Among the available endpoints that perform CRUD, the POST method updates the database dataset to bring data from the spacedevs API. This process usually takes a while because each request brings 1500 records every 100. Wait for the database to populate.
