using Microsoft.AspNetCore;
using NOV.TAT.ProductgRPC.Service;

WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build()
    .Run();



