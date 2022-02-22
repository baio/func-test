module IPA.Startup

//open System
open Microsoft.Azure.Functions.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Serilog
open Microsoft.Extensions.DependencyModel

let private buildConfiguration dir =
  ConfigurationBuilder()
    .SetBasePath(dir)
    .AddJsonFile("settings.json", true)
    .AddJsonFile("local.settings.json", true)
    .AddEnvironmentVariables()
    .Build()
    
// https://github.com/serilog/serilog-settings-configuration#azure-functions-v2-v3
// https://stackoverflow.com/questions/71034036/how-to-setup-serilog-with-azure-functions-v4-correctly
type Startup() =
    inherit FunctionsStartup()

    override this.Configure(builder: IFunctionsHostBuilder) : unit =
       
        
        let ctx = builder.GetContext()
        let functionDependencyContext = DependencyContext.Load(typeof<Startup>.Assembly);
        
        //let config = buildConfiguration ctx.ApplicationRootPath 
        //printfn "222 %O" config
        Log.Logger <-
            LoggerConfiguration()
                .ReadFrom.Configuration(ctx.Configuration,  sectionName = "AzureFunctionsJobHost:Serilog", dependencyContext = functionDependencyContext)
                // .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                // .MinimumLevel.Override("Worker", LogEventLevel.Warning)
                // .MinimumLevel.Override("Host", LogEventLevel.Warning)
                // .MinimumLevel.Override("System", LogEventLevel.Error)
                // .MinimumLevel.Override("Function", LogEventLevel.Debug)
                // .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
                // .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
                //.Enrich.FromLogContext()
                // .WriteTo.Console()
                // .WriteTo.Seq("http://localhost:5341")
                .CreateLogger()

        //builder.Services.AddLogging(fun lb -> lb.AddSerilog(Log.Logger, true) |> ignore)
        //|> ignore

        printfn "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"


[<assembly: FunctionsStartup(typeof<Startup>)>]
do ()
