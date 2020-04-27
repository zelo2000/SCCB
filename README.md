# SCCB
Project for Schedule creation and classroom booking

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