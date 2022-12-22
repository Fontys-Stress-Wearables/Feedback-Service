# Feedback Service
 
 ## Introduction
 The Feedback Service is in charge of managing the feedbacks of a user (adding, updating and removing) based on stress data points.

## Build steps
the build steps are for building the service by itsself with the mongo database & building the all services with changes made to the feedback service specifically.

### Run only service with the Mongo database
1. Pull the project on your device in the IDE you are working with. (I will be using Visual Studio Code)

<img width="960" alt="Screenshot (1407)" src="https://user-images.githubusercontent.com/78371221/209117837-4cff223b-bf0f-41cc-b64c-ce6a9427d196.png">
2.  Locate to the debugger tab as shown image.

<img width="669" alt="Screenshot (1409)" src="https://user-images.githubusercontent.com/78371221/209118235-fb1420b7-406c-480d-b883-cdd9a60387be.png">

3.  Run the Debugger as shown in image.

![image](https://user-images.githubusercontent.com/78371221/209118659-a9604324-0d54-4647-a63b-67d3fd3fefe1.png)
4.  The project is now running locally. 

![Screenshot (1414)](https://user-images.githubusercontent.com/78371221/209119018-271e6929-beca-44ba-b4fc-5994f98d4f6b.png)
5.  To be able to see the endpoints we can use Swagger by adding "/swagger/index.html" to the url. Example "https://localhost:7034/swagger/index.html" as shown in image

### Run Feedback Service with Docker container
1. run "docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/" 
  - p: to reach the mongodb container we set the external and internal port to 27017.
  - v: the volumes specify how we will be storing the mongodb database files.
3. 

### Changes to service and run new image on infrastructure 
- build the image for the backend by running `docker build -f Caregiver-Service/Dockerfile -t <HOST>/caregiver-service:<TAG>`.
- docker build -f Feedback-Service/Dockerfile -t ghcr.io/fontys-stress-wearables/feedback-service:main .
- run "docker compose up -d" on SWSP Infrastructure to run the new image.



when running docker compose the feedback service will run on "http://localhost:7034/feedback".

