using DigitalForms.API;
using DigitalForms.BL.Data;
using DigitalForms.DL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
//using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);
Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
         options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
         //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
     })
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddConfigurations();
//builder.Services.AddDbContext<DFSqlContext>(options =>
//        options.UseSqlServer(configuration.GetConnectionString("DFSqlDb")), ServiceLifetime.Transient, ServiceLifetime.Transient);
//builder.Services.AddDbContext<s255d01dbDfSharedContext>(options =>
//        options.UseSqlServer(configuration.GetConnectionString("DFSqlDb")), ServiceLifetime.Transient, ServiceLifetime.Transient);
builder.Services.AddDbContextPool<DFSqlContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DFSqlDb"),
        sqloptions => sqloptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
            )
        ));
builder.Services.AddDbContextPool<s255d01dbDfSharedContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DFSqlDb"),
          sqloptions => sqloptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
            )
        ));
builder.Services.AddInfrastuctureServices(configuration);
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

app.UseSwagger(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});

//// If OpenAPI v2 is required, update configuration here. Default generation uses OpenAPI v3.
//app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
