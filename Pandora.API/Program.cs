using Microsoft.EntityFrameworkCore;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Application.Extensions;
using Pandora.Infrastructure.Extensions;
using Serilog;
using Serilog.Sinks.PostgreSQL;


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

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration) // appsettings.json'dan okur
           .Enrich.FromLogContext() // Ek log bilgilerini dahil eder
           .WriteTo.Console() // Loglarý konsola yazdýrýr
           .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
         .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PandoraBoxDatabase"), "Logs", needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter()},
            {"message_template", new MessageTemplateColumnWriter()},
            {"level", new LevelColumnWriter()},
            {"time_stamp", new TimestampColumnWriter()},
            {"exception", new ExceptionColumnWriter()},
            {"log_event", new LogEventSerializedColumnWriter()},
        })
           .Enrich.FromLogContext()
           .CreateLogger();

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
