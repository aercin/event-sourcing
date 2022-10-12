using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddConsul();

var app = builder.Build();

var conf = new OcelotPipelineConfiguration()
{
    PreErrorResponderMiddleware = async (ctx, next) =>
    {
        if (ctx.Request.Path.Equals(new PathString("/")))
        {
            await ctx.Response.WriteAsync("{\"status\": \"Healthy\"}");
        }
        else
        {
            await next.Invoke();
        }
    }
};
app.UseOcelot(conf).Wait();

app.Run();


