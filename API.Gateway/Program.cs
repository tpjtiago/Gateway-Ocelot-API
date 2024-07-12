using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ocelot Gateway", Version = "v1" });
});

var app = builder.Build();

app.UseRouting();


app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UsePathBase("/gateway");
app.UseStaticFiles();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.DownstreamSwaggerEndPointBasePath = "/gateway/swagger/docs";
    opt.PathToSwaggerGenerator = "/swagger/docs";
})
.UseOcelot()
.Wait();

app.Run();
