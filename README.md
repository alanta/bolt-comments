# ⚡ Bolt Comments

Simple blog comments with Azure Functions

## About this project

This project provides easy self-hosted comments for your blog on Azure Functions, including a management app running in an Azure Static Web Apps (SWA).
The backing store is Azure Storage Tables.

![screenshot](https://github.com/alanta/bolt-comments/raw/master/src/docs/bolt-screenhost.png "Bolt Admin UI")

## Deploy your own instance

_TODO_

## Integrate with your site

_TODO_

### API based integration

_TODO_

### Integrate with your SSG

_TODO_

### Use WebHooks to update your site

_TODO_

## Running this app on your local machine

Requirements
* Azure Functions CLI
* Azure Static Web Apps CLI (SWA CLI)
* Azure Storeage Emulator
* React tools
* VS Code

## Project layout

* `/api` The API built with Azure Functions 
* `/scr` The front-end application (built with React)
* `/public` Static content for the front-end application.
* `/build` The fully built web app

## App development

In VS Code you can run the app from the terminal. I'm assuming you have the react tools installed as well as the SWA CLI.
From a terminal window start the React dev server:

`yarn run start`

This will launch the react dev server on [http://localhost:3000](http://localhost:3000) . However Azure Static Web Apps use a proxy service to integrate functions, authentication and other features.
So in a new terminal run the full app using the Azure Static WebApp CLI:

`swa start http://localhost:3000 --api ./api`

This should launch the Bolt UI on [http://localhost:4280/](http://localhost:4280/)

## API Development

In VS Code: hit `ctrl-f5` to launch just the functions. Note that authentication relies on Static Web Apps, so you're probably better off running the integrated solution for most dev work.

## Front-end development

The UI is a plain vanilla React app, created with [create-react-app](https://facebook.github.io/create-react-app). In the project directory, you can run:

* `yarn start` Runs the app in the development mode. Open [http://localhost:3000](http://localhost:3000) to view it in the browser.\
  The page will reload if you make edits. You will also see any lint errors in the console.
* `yarn test` Launches the test runner in the interactive watch mode.
* `yarn build` Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.
