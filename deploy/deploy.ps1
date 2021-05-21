param (
    [Parameter( 
        Mandatory=$true,
        HelpMessage="The resouce name of the Azure static web app.")]
    [string]$name,
    [Parameter( 
        Mandatory=$true,
        HelpMessage="The name of the resource group to deploy to.")]
    [string]$resourceGroup,
    [Parameter( 
        Mandatory=$true,
        HelpMessage="The Azure region to deploy to.")]
    [string]$location="westeurope",
    [Parameter( 
        Mandatory=$true,
        HelpMessage="Your GitHub account name.")]
    [string]$githubAccount,
    [Parameter( 
        Mandatory=$true,
        HelpMessage="The GitHub repository name.")]
    [string]$githubRepo,
    [Parameter( 
        Mandatory=$true,
        HelpMessage="The Git branch to use.")]
    [string]$branch="main",
    [Parameter( 
        Mandatory=$true,
        HelpMessage="Your Git Personal Access token. See https://help.github.com/en/articles/creating-a-personal-access-token-for-the-command-line.")]
    [securestring]$token
)

# This assumes you have Azure CLI installed: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
# Make sure you are logged in: https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli
# And have selected the right subscription. See https://docs.microsoft.com/en-us/cli/azure/manage-azure-subscriptions-azure-cli#change-the-active-subscription

az staticwebapp create `
    -n $name `
    -g $resourceGroup `
    -s "https://github.com/$githubAccount/$githubRepo" `
    -l $location `
    -b $branch `
    --app-artifact-location "build" `
    --api-location "/api" `
    --token $token