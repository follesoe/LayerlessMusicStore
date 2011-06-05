# LayerlessMusicStore Demo Application
This repository hosts the demo application presented by Tore Vestues and Jonas Folleso in 
the "*When less is more - Agile web architecture*" talk at NDC2011. The application is a port of (some of) the functionality 
found in the [ASP.NET MVC Music Store example](mvcmusicstore.codeplex.com).
 
The implementation tries to demonstrate a different and simpler approach to architecting web applications. 
The implementation uses [RavenDB](http://ravendb.net/) as a schema less document store, and retrieves the information directly 
using JavaScript and REST without accessing the MVC controllers. Data is written to the database by a 
generic controller responsible for proxying PUT, POST and DELETE requests against the data.

The implementation also demonstrates how you can introduce standard ASP.NET MVC controller and model code 
when needed. The shopping cart is managed by C# controllers as we need server side logic to calculate the 
order total of the cart.

The client side JavaScript code is using [Sammy.js](http://sammyjs.org/) to implement a simple MVC structure to navigate between 
screens of the application.

The implementation should be considered experimental and used to highlight a concept or an idea, 
more than being a reference architecture that should be copied directly. 

## Running the demo application
In order to run the LayerlessMusicStore demo application it should be enough to clone/download the GIT 
repository, open the solution and build it. All dependencies are managed using NuGet, and the solution 
is configured to download external dependencies automatically in a pre-build event 
(so building the first time might take a bit longer than usual).

Once the dependencies are downloaded you need to configure and start RavenDB. In the 
`LayerlessMusicStore\Packages\RavenDB.1.0.0.371\server\` 
folder you need edit the `Raven.Server.exe.config` file and change the `AnonymousAccess` setting key to
`<add key="Raven/AnonymousAccess" value="All"/>`.
