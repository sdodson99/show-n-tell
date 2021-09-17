![Show 'N Tell](https://github.com/sdodson99/show-n-tell/workflows/Show%20'N%20Tell/badge.svg?branch=master&event=push)

# Show 'N Tell

A social media platform to show off your prized possessions. Created for a school project.

# Table of Contents

- [Description](#description)
- [Features](#features)
- [Demo](#demo)
- [Technology](#technology)
  - [API](#api)
  - [Vue Client](#vue-client)
- [How to Run Locally](#how-to-run-locally)
  - [API](#api-1)
  - [Vue Client](#vue-client-1)
- [Deployment](#deployment)
  - [API](#api-2)
  - [Vue Client](#vue-client-2)
- [Contributing](#contributing)

# Description

Show ‘N Tell is a social media platform for users to upload and discover images of other user’s sentimental, valuable, or random items. Many people have interesting items with compelling background stories that they value and think other people would appreciate. Show ‘N Tell allows people to upload images and write stories about their interesting items for the world to recognize.

# Features

- Explore random image posts created by other users
- Upload images with descriptions to share with other users
- Like and comment on other user's image posts
- Explore other user's profiles
- Follow other user's profiles
- View a feed of recent image posts created by followed user profiles
- Real-time feed updates
- Search for image posts
- Add tags to uploaded image posts to increase search rank

# Demo

[Click here](https://youtu.be/FHNkPVIfIvw) for a Show 'N Tell demo featuring several features from above.

# Technology

## API

- ASP.NET Core
- Entity Framework Core w/ SQL Server
- SignalR for real-time updates
- Swagger UI
- Unit Testing w/ NUnit + Moq
- Azure SQL for production database
- Azure Blob Storage for production image storage
- Azure Application Insights for production logging
- Azure Key Vault for storing secrets (keys, connection strings, etc.)
- Azure Event Grid + Azure Function webhook to handle Azure Blob Storage image deletes
- Azure ~~Container Instances~~ App Service to deploy API from Docker image

## Vue Client

- Vue.js
- Vue Router
- Vuex
- Bootstrap
- SignalR

# How to Run Locally

Follow the following steps to run Show 'N Tell locally after cloning.

## API

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

## Vue Client

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

# Deployment

It is **strongly recommended** to successfully run the application locally before attempting to deploy. Deployment steps
**do not** describe the creation or configuration of external services, such as Azure or Surge.

## API

1. Create an Azure Key Vault with the following keys.

- DATABASE-CONNECTION-STRING: For connecting to a production database.
- BLOB-STORAGE-CONNECTION-STRING: For connecting to and storing images in Azure Blob Storage.
- APPLICATION-INSIGHTS-KEY: For logging with application insights.
- ACCESS-MODE: The ShowNTellAccessMode integer value.

2. Build the API Docker image and tag the image with the desired Docker repository or Azure Container Registry URI.

```
docker build --pull --rm -f "src/api/ShowNTell.API/Dockerfile" -t <DOCKER REPOSITORY OR AZURE CONTAINER REGISTRY URI> "src/api"
```

3. Push the tagged Docker image.

```
docker push <DOCKER REPOSITORY OR AZURE CONTAINER REGISTRY URI>
```

4. Create the Azure Container Instance referencing the Docker image.

```
az container create --name <CONTAINER NAME> -g <RESOURCE GROUP> --image <PUSHED IMAGE> --location <LOCATION> --environment-variables KEY_VAULT_NAME=<KEY VAULT NAME> --secure-environment-variables AZURE_CLIENT_ID=<ID> AZURE_TENANT_ID=<ID> AZURE_CLIENT_SECRET=<SECRET>
```

**Note:** Ensure the production environment has credentials to access the Azure Key Vault.

## Vue Client

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

4. Specify your desired domain in the "deploy" script of "package.json".

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

# Contributing

Please create a new issue if you have any questions, problems, or suggestions. Feel free to open a
pull request if you have a feature or fix you want to contribute to the project.
