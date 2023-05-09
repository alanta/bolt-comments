# ⚡ Bolt Comments

Simple blog comments with Azure Functions

## About this project

This project provides easy self-hosted comments for your blog, including a management app. It runs on an Azure Static Web App (SWA) in .NET 6 with Azure Functions v4.
Data is stored is Azure Storage Tables.

### Project Status : _Beta_ ⚠

This project is still in development but it should be safe enough to use for small scale apps.

### Features

* Manage comments for your site
* Comments are cleaned and stored as Markdown
  * Allows images, links and most highlighting
  * Bullet lists
  * Headings 3 through 6 are allowed
* Comments are grouped using a key, usually a url or slug (but that's up to you).
* Webhooks:
  * New comment - fires when a new comment is submitted
  * Comment published - fires when a comments visibilty was changed
* Secured Admin site
* API using API Keys
* Submit directly from an HTML form or through a REST call
* Avatars using Gravatar
* Mass import comments through the API

![screenshot](https://raw.githubusercontent.com/alanta/bolt-comments/main/docs/bolt-screenshot.png "Bolt Admin UI")

### Roadmap


* Post / Reply in the admin UI
* Edit comments in the admin UI
* Sample code for API integration into a site
* Swagger / API docs
* Threaded comments
* Sample code for SSG integration: Statiq, Hugo, Jekyll
* Support for other Avatar services
* Backup & restore
* Spam filter
* A hook to allow comment submisiion to be verified through e-mail
* Easy one-click deploy (not supported by Azure Static Web Apps yet 🙁 )
* Deploy from ARM templates (or Bicep)
* Sign webhook requests so they can be validated

## Deploy your own instance

For now, the easiest way is to fork this repo and deploy it from VS Code.

* [Fork this repo](https://github.com/alanta/bolt-comments/generate) ✨
* Delete the workflow from `.github/workflows` -  the next step will create a new workflow for your instance
* Follow the guide on [creating an Azure Static Web App](https://docs.microsoft.com/en-us/azure/static-web-apps/getting-started?tabs=vanilla-javascript#create-a-static-web-app) to deploy Bolt as a static web app.
  <br>Make sure you select `React` for the defaults and accept other suggestions.
  <br>That should create a fresh deployment workflow in your repo and will start deployment of the static web app.

* Create an Azure Storage Account as a backing store.
* In the Azure Portal, add the connectionstring for the storage account to the appsettings for the static web app:
```json
[
  {
    "name": "DataStorage",
    "value": "DefaultEndpointsProtocol=https;AccountName=***;AccountKey=***;EndpointSuffix=core.windows.net"
  }
]
```
* Now open the _Role management_ tab and create an invitation for yourself with the `admin` role.
* Use the invitation to login and claim your admin role.

Your Bolt Comments instance is now ready to use 🎉.

## Integrate with your site

There are two ways to integrate with your site. You can load the comments directly from the API or while regenerating your site using a Static Site Generator.

### API based integration

_TODO_

#### CORS

Since the API is hosted in a static webapp, you cannot configure CORS. You'll need to 

### Integrate with your SSG

_TODO_

### Use WebHooks to update your site

* Setup a webhook to trigger your (partial) site rebuild.<br>👉 This is not 'in the box', you should use a service like [Zapier](https://zapier.com/), [ITTT](https://ifttt.com/) or [Pipedream](https://pipedream.com) to connect to GitHub, Azure DevOps, Netlify or where ever you host your build.
* Login to your Bolt Comments instance and configure the outgoing web hooks.

The data posted in the webhook looks like this:
```json
{
  "event":"Added",
  "id":"86b08dd47b184a15b1c3edd0002648d5", // the unique id of the comment
  "key":"/posts/cool-stuff",
  "name":"Marnix",
  "email":"marnix@alanta.nl",
  "content":"Nice blog!", // HTML version of the content
  "markdown":"Nice blog!", // Markdown content
  "avatar":"https://www.gravatar.com/avatar/47ab6b5f378e25e3b3ecd2e215ec3c83?d=identicon",
  "posted":"2021-05-21T14:52:12.4030345Z",
  "approved":false
}
```

### Getting notifications

* Setup a webhook to handle notifications to whatever channel you like; e-mail, Slack, Teams etc. <br>
👉 This is not 'in the box', you should use a service like [Zapier](https://zapier.com/), [ITTT](https://ifttt.com/) or [Pipedream](https://pipedream.com).
* Login to your Bolt Comments instance and configure the outgoing web hooks.

```json
{
  "event":"Approved", // or Rejected if the comment is removed after being published
  "id":"e1cd6a359b424139a7bbb9ad1ab88f08", // the unique id of the comment
  "key":"/2021/04/manage-waf-rules-for-appgateway",
  "name":"Marnix",
  "email":"marnix@alanta.nl",
  "content":"Hi!", // HTML version of the content
  "markdown":"Hi!", // Markdown version of the content
  "avatar":"https://www.gravatar.com/avatar/47ab6b5f378e25e3b3ecd2e215ec3c83?d=identicon",
  "posted":"2021-05-06T21:29:51.7603431Z",
  "approved":true // false if rejected
}
```

## Running this app on your local machine

Requirements
* [VS Code](https://code.visualstudio.com/)
* [Azure Static Web Apps CLI](https://www.npmjs.com/package/@azure/static-web-apps-cli) (SWA CLI)
* [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) or [Azure storage emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)


## Project layout

* `/api` The API built with Azure Functions 
* `/scr` The front-end application (built with React)
* `/public` Static content for the front-end application.
* `/build` The fully built web app

## App development

In VS Code you can run the app from the terminal. I'm assuming you have the react tools installed as well as the SWA CLI.
From a terminal window start the React dev server:

`yarn start`

This will launch the react dev server on [http://localhost:1234](http://localhost:1234) . However Azure Static Web Apps use a proxy service to integrate functions, authentication and other features.
So in a new terminal run the full app using the Azure Static WebApp CLI:

`swa start http://localhost:3000 --api-location ./api`

This should launch the Bolt UI on [http://localhost:4280/](http://localhost:4280/)

## API Development

In VS Code: hit `ctrl-f5` to launch just the functions. Note that authentication relies on Static Web Apps, so you're probably better off running the integrated solution for most dev work.

## Front-end development

The UI is a plain vanilla React app. In the project directory, you can run:

* `yarn start` Runs the app in the development mode. Open [http://localhost:1234](http://localhost:1234) to view it in the browser.  
  The page will reload if you make edits. You will also see any lint errors in the console.
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
* The Bolt Comments logo is based on the FontAwesome bolt icon, [licensed](https://fontawesome.com/license/free) under Creative Commons Attribution 4.0 International license. 
