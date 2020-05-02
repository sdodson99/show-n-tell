![Show 'N Tell](https://github.com/sdodson99/show-n-tell/workflows/Show%20'N%20Tell/badge.svg?branch=master&event=push)
# Show 'N Tell

A social media platform to show off your prized possessions.

## Description
Show ‘N Tell is a social media platform for users to upload and discover images of other user’s sentimental, valuable, or random items. Many people have interesting items with compelling background stories that they value and think other people would appreciate. Show ‘N Tell allows people to upload images and write stories about their interesting items for the world to recognize.

## Features
* Explore random image posts created by other users
* Upload images with descriptions to share with other users
* Like and comment on other user's image posts
* Explore other user's profiles 
* Follow other user's profiles
* View a feed of recent image posts created by followed user profiles
* Search for image posts
* Add tags to uploaded image posts to increase search rank

## Demo
[Click here](https://youtu.be/FHNkPVIfIvw) for a Show 'N Tell demo featuring several features from above.

## API Technology
* ASP.NET Core
* Entity Framework Core w/ SQL Server
* Swagger UI
* Unit Testing w/ NUnit + Moq
* Azure SQL for production database
* Azure Blob Storage for production image storage
* Azure Application Insights for production logging
* Azure Event Grid to subscribe to Azure Blob Storage image deletes
* Azure Container Instances to deploy API from Docker image

## Vue Client Technology
* Vue.js
* Vue Router
* Vuex
* Bootstrap

## How to Run Locally
Follow the following steps to run Show 'N Tell locally after cloning.

### API
1. Start the database.
```
docker-compose -f "src/api/ShowNTell.API/docker-compose.yml" up -d --build
```
2. (Optional) Ensure tests pass.
```
dotnet test "src/api"
```
3. Run the application.
```
dotnet run --project "src/api/ShowNTell.API"
```

### Vue Client
You **must** start the API first (see above) in order for the Vue client to work in development.
1. Change directory to "show-n-tell-vue".
```
cd "src/show-n-tell-vue"
```
2. Install packages.
```
npm install
```
3. Run the application.
```
npm run serve
```

## Deployment
It is **strongly recommended** to successfully run the application locally before attempting to deploy. Deployment steps
**do not** describe the creation or configuration of external services, such as Azure or Surge.

### API
1. Add an appsettings.Production.json file to src/api/ShowNTell.API.
2. Configure the appsettings.Production.json file.
```json
{
    "APPLICATION_INSIGHTS_KEY": "<KEY>", // For logging with Azure Application Insights.
    "DATABASE": "<CONNECTIONG STRING>", // For connecting to a SQL Server database with Entity Framework.
    "BLOB_STORAGE": "<KEY>", // For storing images in Azure Blob Storage.
    "IMAGE_BLOB_DELETE_TOKEN": "<TOKEN>", // Optional, for authenticating image delete events from Azure Blob Storage.
}
```
3. Build the API Docker image and tag the image with the desired Docker repository or Azure Container Registry URI.
```
docker build --pull --rm -f "src/api/Dockerfile" -t <DOCKER REPOSITORY OR AZURE CONTAINER REGISTRY URI> "src/api"
```
4. Push the tagged Docker image.
```
docker push <DOCKER REPOSITORY OR AZURE CONTAINER REGISTRY URI>
```
5. Restart the Azure Container Instance referencing the Docker image.
```
az container restart --name <CONTAINER NAME> --resource-group <CONTAINER RESOURCE GROUP NAME>
```

### Vue Client
1. Change directory to "show-n-tell-vue".
```
cd "src/show-n-tell-vue"
```
2. Install packages.
```
npm install
```
3. Build the application.
```
npm run build
```
4. Specify your desired domain in the "deploy" script in "package.json".
```json
{
    "scripts": {
        "deploy": "surge dist/ <DOMAIN>"
    }
}
```
5. Deploy the application to Surge.
```
npm run deploy
```

## Contributing
Please create a new issue if you have any questions, problems, or suggestions. Feel free to open a 
pull request if you have a feature or fix you want to contribute to the project.