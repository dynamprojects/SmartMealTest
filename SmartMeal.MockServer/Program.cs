using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SmartMeal.MockServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
   options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
   options.ListenAnyIP(50051, o => o.Protocols = HttpProtocols.Http2);
});

var app = builder.Build();

app.MapGrpcService<GrpcMockService>();
app.MapControllers();

app.Run();