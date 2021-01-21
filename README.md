A custom user store for ASP .NET Core Identity built in C#. Designed to allow ASP .NET Core Identity to store user and role data in AWS DynamoDB rather than Microsoft SQL Server etc. 

It's designed to be as simple as possible, but it does implement all of the user and role interfaces, except for the IQueryableUserStore. It's fully tested with with XUnit tests and licensed under the MIT license

# Getting Setup
## DynamoDB Tables
You need to have at least one DynamoDB table set up to store users. If you want to use roles, you'll need one for that also. The tables need to have a string type partition key called Id. This project doesn't use secondary indexes at the moment.

## Project Setup
I'm going to assume you're using a freshly bootstrapped ASP .NET Core MVC project with authentication enabled and the options for individual user accounts stored in app are enabled. All this work has already been done in the sample. Feel free to submit an issue if you have problems.

1. Remove the Microsoft.AspNetCore.EntityFramework.Identity NuGet package from the project
2. Install the package into your ASP .NET Core project from NuGet (Package ID will be ara225.DynamoDBUserStore)
3. Replace using statements in the project for Microsoft.AspNetCore.EntityFramework.Identity with using statements for ara225.DynamoDBUserStore
4. Add a using statement for Amazon.DynamoDBv2 to Startup.cs
5. In ConfigureServices in Startup.cs, add a singleton to the services collection containing a DynamoDBDataAccessLayer (a class from ara225.DynamoDBUserStore). You need to pass three arguments into it: an AmazonDynamoDBClient (you can pass a AmazonDynamoDBConfig to this to configure it), a string containing the name of the DyanmoDB table for user storage and a string with the name of the roles table (you can put a null or any string if not using Roles).

To use a local DynamoDB in Docker
```csharp
services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" }), "UserStoreTable", "RoleStoreTable"));
```
To use with DynamoDB in the cloud
```csharp
services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient(), "UserStoreTable", "RoleStoreTable"));
```
6. Add the user store and role store in ConfigureServices in Startup.cs.The options you set in the function passed to AddDefaultIdentity don't really matter for this purpose.

Here's a sample to add both user and role stores to Identity:
```csharp
services.AddDefaultIdentity<DynamoDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddUserStore<DynamoDBUserStore<DynamoDBUser>>()
    .AddRoles<DynamoDBRole>()
    .AddRoleStore<DynamoDBRoleStore<DynamoDBRole>>();
```
Here's a sample to add a user store to Identity:
```csharp
services.AddDefaultIdentity<DynamoDBUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddUserStore<DynamoDBUserStore<DynamoDBUser>>();
```
7. Change the user type in _LoginPartial.cshtml (found in Views/Shared)
Replace 
```csharp
@inject SignInManager<IdentityUser> SignInManager
@inject UserManager<IdentityUser> UserManager
```
with
```csharp
@inject SignInManager<DynamoDBUser> SignInManager
@inject UserManager<DynamoDBUser> UserManager
```

# Running the Sample
I've included a sample ASP .NET Core app in this repo. It's basically just a plain old ASP .Net Core MVC template with this user store instead of the default. It's configured to connect to a DynamoDB running in Docker, though you can change this behavior by setting the environment variable CONNECT_TO_CLOUD_DB to anything. I wrote about setting up a local DynamoDB <a href="https://dev.to/ara225/how-to-run-aws-dynamodb-locally-156i">on dev.to here</a> It creates a test user and a test role on startup.

Aside from that setup, it's just a normal ASP .NET Core app.

# Running the Tests
This project uses tests based on the Microsoft tests for Identity <a href="https://github.com/dotnet/aspnetcore/tree/9699b939f94b7524a178821d78addefa5af5d750/src/Identity/Specification.Tests/src">found here</a>. These have an Apache 2.0 license. They're configured to connect to a DynamoDB running in Docker, though you can change this behavior by setting the environment variable CONNECT_TO_CLOUD_DB to anything.

# Future Plans
This project is fairly complete, and I'm pretty sure it's bug free so there's not much to do. I could implement IQueryableUserStore, but I don't really think it's worth the effort.