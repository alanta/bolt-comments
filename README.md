# ⚡ Bolt Comments

Simple blog comments with Azure Functions

## About this project

This project provides easy self-hosted comments for your blog on Azure Functions, including a management app running in an Azure Static Web Apps (SWA).
The backing store is Azure Storage Tables.

### Project Status : _Alpha_ ⚠

This project is still in development. Some important security concerns should be addressed before using it in 'production'.

### Features

* Manage comments for your site
* Comments are grouped using a key, usually a url or slug (that's up to you).
* Webhooks:
  * New comment - fires when a new comment is submitted
  * Comment published - fires when a comments visibilty was changed
* Secured Admin site
* API using API Keys
* Avatars using Gravatar
* Mass import comments through the API

![screenshot](https://raw.githubusercontent.com/alanta/bolt-comments/main/docs/bolt-screenshot.png "Bolt Admin UI")

### Roadmap

* Clean up commnent HTML on submission
* Post / Reply in the admin UI
* Sample code for API integration into a site
* Swagger / API docs
* Threaded comments
* Sample code for SSG integration: Statiq, Hugo, Jekyll
* Support for other Avatar services
* Backup & restore
* Spam filter

## Deploy your own instance

For now, the easiest way is to fork this repo and deploy it from VS Code.

* Fork this repo
* Delete the workflow from `.github/workflows`
* Follow the guide on [creating an Azure Static Web App](https://docs.microsoft.com/en-us/azure/static-web-apps/getting-started?tabs=vanilla-javascript#create-a-static-web-app) 
  <br>Make sure you select `React` for the defaults.

That should create a fresh deployment workflow in your repo.

## Integrate with your site

There are two ways to integrate with your site. You can load the comments directly from the API or while regenerating your site using a Static Site Generator.

### API based integration

_TODO_

### Integrate with your SSG

_TODO_

### Use WebHooks to update your site

* Setup a webhook to trigger your (partial) site rebuild.<br>👉 This is not 'in the box', you should use a service like [Zapier](https://zapier.com/), [ITTT](https://ifttt.com/) or [Pipedream](https://pipedream.com) to connect to GitHub, Azure DevOps, Netlify or where ever you host your build.
* Login to your Bolt Comments instance and configure the outgoing web hooks.

_TODO :  API docs, inluding the data this hook produces_

### Getting notifications

* Setup a webhook to handle notifications to whatever channel you like; e-mail, Slack, Teams etc. <br>
👉 This is not 'in the box', you should use a service like [Zapier](https://zapier.com/), [ITTT](https://ifttt.com/) or [Pipedream](https://pipedream.com).
* Login to your Bolt Comments instance and configure the outgoing web hooks.

_TODO : API docs, inluding the data this hook produces_

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

`yarn start`

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

## Verifying SWA configuration

_TODO_

## FAQ

### Why do I need an extra service to receive e-mails?
The intent of this project is to provide a simple but useful service. Integrating with an e-mail provider is not standardized and it seems a bit excessive to deploy, for example, a SendGrid instance just for this app.
Instead you can configure a web hook and use a service like [Zapier](https://zapier.com/), [ITTT](https://ifttt.com/) or [Pipedream](https://pipedream.com).

### Why are webhooks not out-of-band with retries?
Azure Static Web Apps [do not support any other triggers than HTTP]() on the functions. This makes it really hard to do anthing but service incoming requests from the built-in functions. We may decide to host the functions in a separate function project in the future. See Azure/static-web-apps#389 and #2.

## Credits

* The UI is based on Bootstrap and the [Anchor UI Kit](https://wowthemesnet.github.io/Anchor-Bootstrap-UI-Kit/index.html) from Wowthemes.
