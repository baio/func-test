module Run

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

[<FunctionName "queries">]
let queries ([<HttpTrigger "POST">] req: HttpRequest, logger: ILogger) =
    {| ok = true |}
