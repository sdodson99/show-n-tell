![Show 'N Tell](https://github.com/sdodson99/show-n-tell/workflows/Show%20'N%20Tell/badge.svg?branch=master&event=push)
# Show 'N Tell

A social media platform to show off your prized possessions.

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
