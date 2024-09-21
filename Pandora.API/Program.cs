using Microsoft.EntityFrameworkCore;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Application.Extensions;
using Pandora.Infrastructure.Extensions;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
//builder.Services.AddCoreServices();
//builder.Services.AddCrossCuttingConcernsServices();

builder.Services.AddDbContext<PandoraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PandoraBoxDatabase")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PandoraBoxDatabase"), "Logs", needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"Message", new RenderedMessageColumnWriter()},
            {"MessageTemplate", new MessageTemplateColumnWriter()},
            {"Level", new LevelColumnWriter()},
            {"TimeStamp", new TimestampColumnWriter()},
            {"Exception", new ExceptionColumnWriter()},
            {"LogEvent", new LogEventSerializedColumnWriter()},        
        })  
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
