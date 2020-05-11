# SCCB
Project for Schedule Creation and Classroom Booking

## Technologies

* ASP.NET Core 3.1 
* MSSQL local db.

## Set up

* Open solution in Visual Studio 2019
* Set connection string in appsettings.json
* Restoring client side libraries
* Set SCCB.Web project as Startup Project and build it.
* In package manager console write `update-database`
* Run!

## Documentation

Documentation can be find [here](docs/Documentation.pdf). 
This includes lots of information and will be helpful if you want to understand the features Schedule Creating and Classroom Booking currently offers.

## How to conect logger.
To add logging with Serilog install next NuGet packages:
- Serilog.AspNetCore
- Serilog.Settings.Configuration

In file appsettings.json in "Serilog" -> "WriteTo" -> "Args" -> "path" you have to write
your own path to a folder Logs
- "D:\\6th term\\ПІ\\SCCB\\SCCB\\Logs\\log.txt"
- "D:\\6th term\\ПІ\\SCCB\\SCCB\\Logs\\log.json"

To launch Seq Server:
- download Seq here https://datalust.co/Download
- install the Seq server
- install Serilog.Sinks.Seq NuGet package and add it to your application
- start Seq server on your computer by launching Seq.Administration.exe
- go to http://localhost:5341
