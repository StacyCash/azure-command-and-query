---
published: true
title: "Setting up the Azure Environment"
cover_image: "https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/header-image.jpg"
description:
tags: azure, storageaccount, azurefunctions, tutorial
series: "Azure Command and Query"
canonical_url: https://dev.to/stacy_cash/setting-up-the-azure-environment-47fb
---

* Step 1: [Getting Started with Azure Command and Query](https://dev.to/stacy_cash/getting-started-with-azure-command-and-query-80j)
* Step 2 : Setting up the Azure Environment

Coding for the cloud can seem a mountainous challenge at the start. What resources do you need, how can you best use them, and just what will it cost to run your solution?

In our [previous step](https://dev.to/stacy_cash/getting-started-with-azure-command-and-query-80j) we built an application that exposes a WebAPI to receive new subscriptions to a book club. 

This is then stored in an Azure storage account queue, so that the user can continue whilst we process the request further using an Azure Function.

The Azure function is triggered by the queue, and writes the data to table storage in the same storage account for us.

To aid our development, and to allow us to focus, we used the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) to fake our storage account, and ran both the WebAPI and Function with 'F5' using the standard Visual Studio tooling to allow them to run locally.

This is great for development and experimentation. But we need to deploy at some point.

In this tutorial we are going to create an environment in Azure using the Azure Portal, and publish our code for  testing.

## Getting Started

You can find the starting point in the GitHub repo [here](https://github.com/StacyCash/azure-command-and-query)

Clone the repo, if you haven't already and checkout `step-two-start`

## The Azure Portal

Before we can make the changes that we need to in our code, we first need an environment to communicate with, and deploy to.

> **Note**: For these steps you need a Microsoft Account. If you do not have one with access to Azure resources then you will need to create one. We are not covering that in this tutorial, but you can read more [here](https://account.microsoft.com/account?lang=en-us)

1. Open [portal.azure.com](portal.azure.com)
2. Login into your Microsoft Account

We are now on the home page of the Azure Portal. Amongst other things from here we can check the state of your account, navigate around current resources and create new resources. It's the later that is important for us today.

## Resource Groups

The first thing that we need to set up is an environment is a resource group. As the name implies a resource group ties the resources together in one group.

This allows easy management of the resources. All related resources are located together, the costs for all related resources are grouped, and when the time comes to clean up your environment all are located together.

1. Click on the 'Resource Groups' icon

![Resource Group Icon](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-icon.png)

2. On the Resource Groups page click the '+ Create' button.

![Resource Group List](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-groups-list.png)

3. Select the subscription to attach the resource group to
4. Give your resource group a meaningful name
5. Select the region to store the metadata for your resource group

![New Resource Group Blade](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/new-resource-group-blade.png)

6. Click the 'Review and Create' button

![Review and Create Button](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-review-and-create.png)

7. Check that the data entered is correct and press 'Create'

![Create Resource Group](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-review-tab.png)

Azure will now create your resource group for you

8. Refresh the Resource Group List and you should now see your resource group

![New Resource Group in Resource Group List](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/new-resource-groups-list.png)

Most resources take some time to create, the resource group is an exception to this and appears almost immediately.

## Creating the Web App

Now we need to add something to it, the first thing that we need to be able to deploy our app is a `Web App`.

We need a Web App in order to deploy our Web API application. Think of this like the IIS of a Windows Server.

1. Go to the page for the resource group that we just made

![New Resource Group Page In Azure Portal](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-empty.png)

2. Click on the `Add`

![Resource Add Button](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/add.png)

3. Click on `Web app'

![New Resource Blade](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/new-resource.png)

![Web App Creation Link](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/web-app-create-link.png)

> Click on the `Web App` link, otherwise we'll find ourselves in the quick start page 😉

4. We should now be on the page to create a new web app - it should look like this

![Web App Creation Blade](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/web-app-creation-blade.png)

5. Make sure that the right subscription is selected
6. Make sure that the right resource group is selected (if you created the resource from the resource group this will be prefilled)
7. Give your Web App a name

> This name cannot contain spaces, and needs to be unique, globally. If someone else has used this name already you will get an error message

![Web App Name Not Available](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/web-app-name-not-available.png)

8. For publishing click `Code`
9. Runtime stack is `.Net 3.1 LTS`
10. Select `Windows` for the Operating System

> **Note**: There are limitations with Linux App Service plans when using consumption based Function Apps (as we will later in this tutorial), so we are using Windows. If we were to use two Resource Groups to separate this App Service plan from the Function App then we could use Linux for our hosting. For more information see [this wiki](https://github.com/Azure/Azure-Functions/wiki/Creating-Function-Apps-in-an-existing-Resource-Group)

11. For region select the same as your Resource Group

> **Note**: Depending on the subscription that you have you may not be able to select every region. In which case `Central US` - this one has always worked for me

### App Service Plan

That is all of the settings that we'll be using today for the Web App itself, but you may have noticed that there are still some settings that we need to look at.

![App Service Plan](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/app-service-plan-empty.png)

The Web App needs somewhere to run. This is the App Service. If you think of the Web App as IIS, then think of the App Service as the machine it runs on.

As we don't have an App Service in our Resource Group we are going to need to create one.

1. Click the `Create New` link

![App Service Create Link](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/app-service-create-link.png)

2. In the pop-up that opens fill in the name for your app service

![New App Service Name](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/app-service-create-new.png)

3. Fill in a name (this does not have to be globally unique)
4. Click `OK`
5. Now we want to select the SKU and size of our App Service.
6. Click on `Change Size`

![App Service Change Size Link](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/app-service-change-size-link.png)

We should now have a new fly-in open, allowing up to pick what specifications we want for our App Service

![App Service Select SKU](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/app-service-select-sku.png)

Here we can find all of the options available to us for development, testing, production and isolated instances. Have a look around to see what is available

7. Pick Dev / Test

8. Pick `F1 Shared Infrastructure`. For our demo free is good enough!

   > For practice, and demonstrations I always use the Dev/Test F1 tier. This is free, has 60 minutes run time a day and is good enough for what we are doing today. 
   >
   > 60 minutes a day does not mean it is only available for 60 minutes a day. It means you only get 60 minutes of actual compute time. If our service is only busy for 1 minute per hour then you would only use 24 in that day even though it was available for the whole 24 hours.

9. Click apply

We should now have a screen in front of us that looks a little like:

![Web App Creation Complete](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/web-app-creation-blade-complete.png)

10. Click `Review + Create`
11. Check that everything on the review page looks OK

![Web App Creation Check](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/web-app-creation-blade-check.png)

12. Click `Create`

Azure will now create the resources for us, this will take a few minutes.

## Creating the Azure Storage Account

The Web App allows us to host our API, but now we need some storage for it to talk to.

1. Go back to the page for the resource group, now with an App Service and Web App

![New Resource Group Page In Azure Portal](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-empty.png)

2. Click on the `Add` button to open the new resource blade

![Resource Add Button](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/add.png)

![New Resource Blade](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/new-resource.png)

3. Search for `Storage Account`

![Search for Storage Account](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/new-resource-storage-account-search.png)

4. Click on the `Storage Account - blob, file, table, queue` option

5. In the marketplace page that opens, click on `Create`

![Storage Account Marketplace Page](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-marketplace-page.png)

6. In the creation screen make sure that the subscription and resource group are correct

![Storage Account Creation - Subscription and Resource Group](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-creation-subscription-and-resrouce-group.png)

7. Fill in the `Instance Details' using this information as a guide

![Storage Account Creation - Instance Details](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-creation-instance-details.png)

8. Click `Review and Create`

![Storage Account Review and Create](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-review-and-create.png)

9. Check that the validation has passed, and that all of information is as you intended it to be

![Storage Account Validation and Review](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-validation.png)

10. Click `Create`

![Storage Account Create](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-create.png)

Azure will now provision our storage account for us!

Whilst it's doing that lets take a quick look at those `Instance Details

### Storage Account Name

The storage account name is a globally unique name within Azure.

The same as our Web App, this means that we need to pick a name here that no one else has used.

Unlike the Web App, we have more limitations with the name. 

The only characters allowed are lowercase letters and numbers. No PascalCase, camelCase or kebab-case names are allowed.

Yes, this makes the name harder to read. Sorry.

### Location

There are two rules of thumb here:

* Keep it close to the metal where it will be written and read.
* Make sure that your users data is compliant with local rules regarding data location.

### Performance

For this example we do not need a high data throughput, or extreme response times, so the cheaper standard performance is good enough.

### Account Kind

There are three account kinds:

* Storage V2 (general purpose v2)
* Storage (general purpose v1)
* BlobStorage

The Storage V2 accounts are general purpose accounts for File, Table, Queue and Blob storage. For general purpose use this is what we should always use. V1 accounts should only be used for legacy applications that need it. BlobStorage accounts are specialised for high performance block blobs, not for general purpose use.

### Replication

Storage accounts always replicate data, but you can specify different levels of replication. This comes at a price, the further down this list you go, the more you pay.

* LRS: Cheapest, locally redundant in 1 data center
* ZRS: Redundant across multiple data centers in one zone
* GRS: Redundant across two zones
* RA-GRS: As above, but with read access across the zones
* GZRS: Redundant across multiple data centers in the first zone, and a single data center in the second
* RA-GZRS: As above, but with read access across the zones

## Updating the WebAPI with the Storage Account

Now that we have an Azure Storage Account we can start to use it, to do so we need to make some changes to our WebAPI application.

### Get the connection string for the Storage Account

1. Open the resource group
2. Click on the Azure Storage Account created in the last step to open the resource

![Storage Account in Resource Group List](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-in-resource-group-list.png)

3. Click on 'Access Keys'

![Storage Account Resource Menu.png](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-resource-menu.png)

> The Access Keys are in the `Settings` section

![Storage Account Access Key Menu Item](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-access-key-menu-entry.png)

4. On the screen that opens we can see two keys and connection strings. Copy one of the connection strings

![Azure Storage Account Keys and Connection Strings](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-access-keys.png)

> You can do this easily using the copy button next to the connection string

![Connection String Copy Button Highlighted](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/storage-account-access-keys.-copy-button.png)

> **Note**: Do not post  keys online, the storage account is open for attack with these keys available. The keys seen here are no longer valid, which is why they are shared for demonstration purposes 😉

### Update the WebAPI with the copied connection string

1. Open the WebAPI code
2. Open the `QueueAccess.cs` file
3. Replace the `_connectionString` string literal with the connection string copied from the Azure Portal

It should look like this, but with your storage account connection string:

``` C#
  public class QueueAccess
  {
    private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mystor...";
```

### Test the WebAPI and Queue

We are now ready to run!

1. Start the WebAPI in debug mode
2. Open the following folder in a terminal (command prompt, Windows PowerShell, Bash etc). The folder is relative to the base folder of the git repo for this tutorial)

`<git repo folder>\front-end`

3. Start the Angular front end application by running: `ng serve`

> **Note**: For this you need to have npm and the angular CLI installed. We are not covering that in this tutorial, but you can find more information here [Angular CLI](https://cli.angular.io/) and here [npm and NodeJS](https://nodejs.org/en/).

4. Browse to `http://localhost:4200`
5 Fill in a name, email and preferred genre
6. Click `Submit`

![Book club sign up](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/book-sign-up-site.png)

7. Open the Azure Portal
8. Go to the Azure Storage Account we created
9. Open the Storage Explorer from the side bar

![Azure Portal Storage Explorer Link](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/azure-portal-storage-explorer.png)

> **Note**: The Azure storage explorer is still in preview, as such there may be some issues. But for our example here it works without a problem.

10. Open the `Queues` \ `bookclubsignups`
11. We can see our sign up data in the queue!

![Azure Portal Storage Explorer with Queue Data](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/azure-portal-storage-explorer-queue.png)

## Deploying the WebAPI to Azure

So now we have all of the resources that we need in Azure to host our WebAPI and to hold our queue, and we have a WebAPI that can send messages into our queue.

It's time to deploy our code and see it work in the wild!

1. Open the `SignupApi` solution in Visual Studio
2. Right click on the `SignupApi` project

![Visual Studio Right Click SignupApi Project](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-right-click-project.png)

3. Click `Publish`

![Publish menu item](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-publish-context-menu.png)

> **Note**: There are several notable public speakers who use the phrase *friends don't let friends right click publish*. But for experimentation and a quick deploy it's very useful!

4. In the window that opens select 'Azure'

![Right Click Publish Step 1](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-step-1.png)

5. In the 2nd step select `Azure App Service (Windows)`

![Right Click Publish Step 2](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-step-2.png)

6. In the 3rd step select the resource group inside your Azure subscription, and pick the WebApp from the tree view.

![Right Click Publish Step 3](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-step-3.png)

7. We now have our publish profile set up. We also have the option for setting up our storage account, which has been detected, and even a pipeline for CI deployments. But for now we are simply going to press the `Publish` button

![Visual Studio Publish Screen](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-publish-profile.png)

Visual Studio will run some checks to ensure that our code will run in the cloud, build, publish and push our code to Azure, and when everything is complete open a browser with the URL of the service.

Don't worry that this shows an error, we only have one route set up for our WebAPI, so the root won't show anything...

***But do copy the URL!***

### Testing the Azure Service

We can test that our API is working using our front end application.

1. Open the front end angular project folder in VS Code

`<git rep folder>\front-end`

2. Open the `SubscriptionService` class

`src\app\service\subscription.service.ts`

3. Change the URL for the API from localhost to the deployed web app

From:
``` ts
const url = `https://localhost:44343/api/bookclubsignup`;
```

To:
``` ts
const url = `https://<webservicename>.azurewebsites.net/api/bookclubsignup`;
```

4. Run the Angular app and add a new sign up the the book club
5. Recheck the Azure Storage Account and check that your new signup is saved, below you can see Rory has signed up for book club now as well

![Azure Storage Explorer With Second Request](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/azure-portal-storage-explorer-queue-after-deploy.png)

## Creating the Azure Function

We now have a working WebAPI, deployed and sending requests to the queue. Now we need to read that queue and store the data in a table.

To do that we need to deploy our Azure Function, so we need a new resource.

1. Open the Azure Portal
2. Go to the Resource Group
3. On the Resource Groups page click the '+ Add' button
4. Click on the `Function App` link (not `Quick starts + tutorials`)

![Function App Icon](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/azure-resource-add-function-app.png)

5. In the Create Function App Basics make sure the correct subscription and resource group are selected

6. Give the function app a name, this has to be unique in Azure

7. For `Publish` select `Code`

8. Pick `.NET Core` for the `Runtime Stack`

9. Select `3.1` for `Version`

10. Make sure that the Function App is located in the same `Region` as the Storage Account and Resource group

![Function App Creation Basics](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-basics.png)

11. Click 'Next: Hosting >'

![Function App Creation Next Hosting](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-next-hosting.png)

12. For the `Storage Account` pick the one we created in this tutorial

> **Note**: If this isn't available then double check your regions - you can only pick storage accounts in the same region as you are creating the Function App

13. Pick `Windows` for the operating system

> **Note**: As said earlier, there are limitations with Linux App Service plans when using consumption based Function Apps, so we are using Windows. If we were to use two Resource Groups to separate this App Service plan from the Function App then we could use Linux for our hosting. For more information see [this wiki](https://github.com/Azure/Azure-Functions/wiki/Creating-Function-Apps-in-an-existing-Resource-Group)

14. For the `Plan Type` pick `Consumption (Serverless)`

![Function App Creation Hosting](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-hosting.png)

15. Click `Review + Create`

![Function App Creation Review And Create](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-review-and-create.png)

16. Check the details of the Function App and if all are correct click `Create`

![Function App Creation Review](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-review.png)

![Function App Creation Create](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-creation-create.png)


Azure will now deploy the resources needed for our new Function App

Once deployed, our resource group is now complete, and should look like this

![Completed Resource Group](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/resource-group-complete.png)

The function app creation also created the `Application Insights` resource, but we are not going to be using that for this tutorial, so we can ignore it.

We can also see the new App Service Plan that has been created for the Function App. This will be spun up when the Function App is called, and the code for the function deployed to it from the Storage Account.

> **Note**: This does mean that there is a delay when calling a consumption based Function App from cold before it responds. This is why we have used a regular WebAPI hosted in a Web App for our API layer.

## Updating the Function App with the Storage Account

Now that we have our Function App available we can update our Function App code to to triggered from our queue, and to write our data to the queue.

### Updating the Function App trigger to use the Storage Account

There are three changes that we need to make to change the trigger

1. Open the Function App solution
2. Open the `serviceDependencies.local.json` file

![Function App Solution Service Dependencies Local JSON](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-solution-service-dependencies-local-json.png)

3. Copy the JSON below into the file

``` json
{
  "dependencies": {
    "storage1": {
      "resourceId": "/subscriptions/[parameters('subscriptionId')]/resourceGroups/[parameters('resourceGroup')]/providers/Microsoft.Storage/storageAccounts/<Storage Account Name>",
      "type": "storage.azure",
      "connectionId": "AzureWebJobsStorage"
    }
  }
}
```

> **Note**: The `resourceId` ends with the Storage Account of our queue. In our example we used `mystorageaccounttutorial`, that is what we would use here. Whatever the name of the Storage Account is should be used.

4. Copy the connection Azure Storage Account connection string from the Azure Portal, [as we did earlier](#Get-the-connection-string-for-the-storage-account)
5. Open the `local.settings.json` file

> You may need to recreate this file due to `.gitignore` settings. If so create it in the root of the Functions project

6. Replace the value of `AzureWebJobsStorage` (currently `"UseDevelopmentStorage=true"`) with the value copied from the Azure portal

It should now look like this, but with your storage account connection string:

``` json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=mystor...",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

> **Note**: This setting allows us to run the solution locally, but using the Azure Storage Account rather than the Storage Emulator

7. Finally, open the `StorageTableAccess.cs` file
8. Replace the `_connectionString` string literal with the connection string copied from the Azure Portal

It should look like this, but with your storage account connection string:

``` C#
  public class StorageTableAccess
  {
    private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mystor...";
```

### Test the full application locally

We can now do a full local test! From web application, through our WebAPI, Azure Function running locally and into our Storage Table.

1. Start the function in Debug mode

> **NOTE:** Function Apps run in specialised environments in Azure, when you run your app in debug mode Visual Studio spins up an Azure Function Tools application to create this environment locally.

2. Run through the same same steps as our [previous test](#test-the-webapi-and-queue) - *but don't look in the `Queue` section!*
3. Instead, look in the `Table` section, where we should now see our latest test:

![Azure Portal Storage Explorer Table](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/azure-portal-storage-explorer-table.png)

## Deploying the Function App to Azure

Now on to the last step, deploying our `BookClubSignupProcessor` to Azure!

This flow is similar to [deploying the WebAPI to azure](#deploying-the-webapi-to-azure)

1. Open the `BookClubSignupProcessor` solution
2. Right click on the `BookClubSignupProcessor` project

![Visual Studio Right Click SignupApi Project](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-right-click-project-functiont.png)

3. Click `Publish`

![Publish menu item](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-publish-context-menu.png)

4. In the window that opens select 'Azure'

![Right Click Publish Step 1](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-function-step-1.png)

5. In the 2nd step select `Azure Function App (Windows)`

![Right Click Publish Step 2](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-function-step-2.png)

6. In the 3rd step select the resource group inside your Azure subscription, and pick the Azure Function from the tree view.

![Right Click Publish Step 3](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/right-click-publish-menu-function-step-3.png)

7. We now have our publish profile set up. We also have the option for setting up our storage account, which has been detected, and even a pipeline for CI deployments. But for now we are simply going to press `Publish`

![Visual Studio Publish Screen](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/visual-studio-publish-profile-function.png)

> **Note:** Here we see a big difference from the WebAPP deploy. The Azure Function has a dependency on the Storage Account, which has a warning. We are going to ignore it for now

![Publish Button With Warning](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/publish-with-warning.png)

8. Click the `Publish` button

Now it's deployed, we can take a look at the function in the Azure Portal to ensure that it worked correctly.

### Check that the function has been deployed and is available

Now that we have deployed our function app we can see it in the Azure portal

> **NOTE:** Functions take time to spin up, it may be that after you have deployed it doesn't appear straight away

1. Open the Azure Portal
2. Go to the Resource Group
3. In the list of resources click on the Function App created

![Function App in Resource Group List](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-in-resource-group.png)

4. In the side menu of the Function App click `Functions` in the group `Functions`

![Function App Side Menu](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-app-side-menu.png)

5. In the blade that opens we can see all of the functions within the Function App, the type of trigger and if they are enabled

![Function List](https://raw.githubusercontent.com/StacyCash/azure-command-and-query/main/blog-posts/step-two/Images/function-list.png)

Now that we know our Function is deployed and available we can run our final test!

### Test the deployed Function App

We now have a full environment deployed and can do one final test to make sure that everything is set up as it should be.

We are going to use the same test as we did running the [Function locally](#test-the-full-application-locally). The only thing we need to run locally now is our front end!

## Closure and Next Steps

You can find the end point in the GitHub repo [here](https://github.com/StacyCash/azure-command-and-query)

Clone the repo, if you haven't already and checkout `step-three-start`

> Files will need editing to run - ensure that the correct connection strings and storage account names are set. Due to the nature of publish profiles and their access tokens these files are not included - you will need to follow the publish steps yourself to deploy this repo.


Our solution is now deployed, and running in the cloud. Using a Web App, Azure Storage Account and Azure Function to run in the wild.

As with our previous work, this has been a quick skim through, and is just the start of making a maintainable cloud solution. In the following posts, over the coming months, we'll be

* Taking a look at the Azure cost calculator so that we can check what the associated costs of that environment will be
* Taking a deeper dive into each of the Azure resources we need for this experiment
* Taking a deeper dive into each of the APIs that we are using to access them!
* Finally, we'll be automating the deployment, using Azure DevOps, and quickly throwing a static Angular site into the air so that we can interact with our API

## Further Reading

### Azure Web App

* [Microsoft Learn App Service](https://docs.microsoft.com/en-us/learn/modules/host-a-web-app-with-azure-app-service/?WT.mc_id=AZ-MVP-5003925)
* [Microsoft Docs App Service](https://docs.microsoft.com/en-us/azure/app-service/?WT.mc_id=AZ-MVP-5003925)

### Azure Storage

* [Microsoft Learn Storage Accounts](https://docs.microsoft.com/en-us/learn/modules/create-azure-storage-account/?WT.mc_id=AZ-MVP-5003925)
* [Microsoft Docs Storage Accounts](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview?WT.mc_id=AZ-MVP-5003925)

### Azure Functions

* [Microsoft Learn Azure Functions](https://docs.microsoft.com/en-us/learn/modules/create-serverless-logic-with-azure-functions/?WT.mc_id=AZ-MVP-5003925)
* [Microsoft Docs Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/?WT.mc_id=AZ-MVP-5003925?WT.mc_id=AZ-MVP-5003925)

[Cover photo by Philipp Birmes from Pexels](https://www.pexels.com/photo/low-angle-photo-of-four-high-rise-curtain-wall-buildings-under-white-clouds-and-blue-sky-830891/)
