
# Feedback Service
 
 ## Introduction
 The Feedback Service is in charge of managing the feedbacks of a user (adding, updating and removing) based on stress data points.

This readme file will cover the endpoints of the service, running the service with its database on docker and lastly, how to run the SWSP infrastructure with new changes made to the service with building a new docker image of the service.

## Endpoints
 ![Screenshot (1414)](https://user-images.githubusercontent.com/78371221/209119018-271e6929-beca-44ba-b4fc-5994f98d4f6b.png)
<i>Image 1: Endpoints of the Feedback Service in Swagger<i>

| Endpoints | Description | 
|--|--|
|(GET) /feedback | Retrieves all feedbacks registered in the database. |
|(POST) /feedback| creates a new feedback |
|(GET) /feedback/{id}|retrieves a specific feedback by its id.|
| (PUT) /feedback/{id} | updates a feedback by id. |
|(DELETE) /feedback/{id}| deletes a feedback by id. |
|(GET) /feedback/patient/{patientId} |retrieves all feedbacks from a specific patient based on their patient id.|
| (GET) /feedback/timespan/{patientId} |retrieves all feedbacks from a specific patient based on their patient id and timespan indicated.|


## Build steps for running service on Docker
The build steps for running the service with the mongo database on Docker. 

<img width="940" alt="mongo-command" src="https://user-images.githubusercontent.com/78371221/211282427-767ffabf-65bf-4e71-bfbb-f0fb5734d207.PNG">
<i>Image 2: Command to create mongo container<i>
 
1. Pull the mongo image from docker with the assigned port and volume to store data with using the following command "Docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo"

  - p: to reach the mongodb container we set the external and internal port to 27017.
  - v: the volumes specify how we will be storing the mongodb database files.
 
![docker-ps](https://user-images.githubusercontent.com/78371221/211282275-7c3c9c75-3d08-4c8b-b0d7-964865bc2bec.png)
<i>Image 3: command to confirm image is running<i>
 
2. Confirm that the docker image is being used "docker ps"


3. Pull the project from GitHub on your device in the IDE you are working with. (I will be using Visual Studio Code)

<img width="960" alt="Screenshot (1407)" src="https://user-images.githubusercontent.com/78371221/209117837-4cff223b-bf0f-41cc-b64c-ce6a9427d196.png">
<i>Image 4: VSCode build of service<i>
 
4.  Build the project in the IDE of choice. 


![image](https://user-images.githubusercontent.com/78371221/209118659-a9604324-0d54-4647-a63b-67d3fd3fefe1.png)
<i>Image 5: Feedback service running locally<i>
 
5.  The project is now running locally and should be able to be found at https://localhost:7034. 

![Screenshot (1414)](https://user-images.githubusercontent.com/78371221/209119018-271e6929-beca-44ba-b4fc-5994f98d4f6b.png)
<i>Image 6: Swagger page of the Feedback service<i>
 
6.  To be able to see the endpoints we can use Swagger by adding "/swagger/index.html" to the url. Example "https://localhost:7034/swagger/index.html" as shown in image


## Build steps for running new image of service on SWSP Infrastructure
Building a new Dockerfile with changes made to the Feedback service and running it with the SWSP infrastructure.

1. docker build -f Feedback-Service/Dockerfile -t ghcr.io/fontys-stress-wearables/feedback-service:main .
2. run "docker compose up -d" on SWSP Infrastructure to run the new image.
