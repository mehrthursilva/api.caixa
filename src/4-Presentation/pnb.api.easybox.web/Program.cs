using AutoMapper;
using Microsoft.Extensions.Options;
using pnb.api.easybox.dependencyinjection;
using pnb.api.easybox.domain.Mapping;
using pnb.api.easybox.repository.Configuration;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDependencies();

// Add services to the container.

var dbConfiguration = new ConfigurationRepoitory();
new ConfigureFromConfigurationOptions<ConfigurationRepoitory>(
    builder.Configuration.GetSection("ConnectionStrings"))
    .Configure(dbConfiguration);
builder.Services.AddSingleton(dbConfiguration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200/")) //http://localhost:9200/
    {
        AutoRegisterTemplate = true,
    })
    .CreateLogger();

     builder.Services.AddLogging(b=>b.AddSerilog());

     var mappingConfig = new MapperConfiguration(mc =>
     {
         mc.AddProfile<AutomapperConfiguration>();
     });
     IMapper mapper = mappingConfig.CreateMapper();
     builder.Services.AddSingleton(mapper);

     var app = builder.Build();

        /*INICIO DA CONFIGURAÇÃO - PROMETHEUS*/
        // Custom Metrics to count requests for each endpoint and the method
        var counter = Metrics.CreateCounter("webapimetric", "Counts requests to the WebApiMetrics API endpoints",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" }
            });

        app.Use((context, next) =>
        {
            counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
            return next();
        });

        // Use the prometheus middleware
        app.UseMetricServer();
        app.UseHttpMetrics();

        /*FIM DA CONFIGURAÇÃO - PROMETHEUS*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
