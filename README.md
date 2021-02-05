# Getting Started with Azure Command and Query

Coding for the cloud can seem a mountainous challenge at the start. What resources do you need, how can you best use them, and just what will it cost to run your solution?

In this walkthrough we'll make a simple application that can be used as a kick-off point for building a disconnected way to store data from our users for future processing.

We'll use a WebAPI to get data from the user, a queue to disconnect the user from the processing, a function to read the queue and, finally, table storage to safely store our users data.

We are going to use a static Angular app as the front end. A single page that gathers three bits if information about the user and passes it to the WebAPI. This post doesn't cover that app, but you can find the code in the repo for this tutorial.

## Our journey begins

The code in this repository can be used to follow along the series of tutorials to be found [here](https://dev.to/stacy_cash/getting-started-with-azure-command-and-query-80j)
