using Microsoft.EntityFrameworkCore;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Application.Extensions;
using Pandora.Infrastructure.Extensions;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Net;
using Pandora.API.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // Token süresi tam dolduğunda geçersiz olur
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
//builder.Services.AddCoreServices();
//builder.Services.AddCrossCuttingConcernsServices();

builder.Services.AddDbContext<PandoraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PandoraBoxDatabase")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Pandora.API",
            Version = "v1",
            Description = "Pandora API"
        });

    c.EnableAnnotations(); //Swagger açıklama desteği

    // JWT Bearer token için Swagger ayarları
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigins", policy =>
//        policy
//        .WithOrigins("http://localhost:4200") // Angular client URL
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials()); 
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "LoginPolicy", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
    });
});

// HttpContextAccessor for accessing HTTP context in services
builder.Services.AddHttpContextAccessor();

// Later in the pipeline

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
    .Filter.ByExcluding(logEvent => logEvent.MessageTemplate.Text.Contains("The query uses a row limiting operator"))
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Assembly ve Uygulama Bilgileri Loglama
var assembly = typeof(Program).Assembly;
var version = assembly.GetName().Version?.ToString() ?? "Unknown Version";
var assemblyName = assembly.GetName().Name ?? "Unknown Assembly";
var buildDate = File.GetLastWriteTime(assembly.Location);
var startTime = DateTime.Now;

// Sunucu IP adresini alma
var host = Dns.GetHostName();
var serverIpAddress = Dns.GetHostEntry(host)
    .AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "Unknown IP";
var ipAddresses = Dns.GetHostEntry(Dns.GetHostName())
    .AddressList
    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    .Select(ip => ip.ToString())
    .ToList();
var machineName = Environment.MachineName;
var userDomainName = Environment.UserDomainName;

// Loglama
var projectInfo = new
{
    ProjectName = assemblyName,
    Version = version,
    BuildDate = buildDate,
    StartTime = startTime,
    Environment = app.Environment.EnvironmentName,
    OS = Environment.OSVersion.ToString(),
    Runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
    ProcessId = Environment.ProcessId,
    ApplicationPath = assembly.Location,
    UserName = Environment.UserName,
    DomainName = Environment.UserDomainName,
    MachineName = Environment.MachineName,
    ServerIPAddresses = ipAddresses
};

var formatted = JsonSerializer.Serialize(projectInfo, new JsonSerializerOptions
{
    WriteIndented = true,
});

Log.Information("Project Details:\n{ProjectDetails}", formatted);

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
  
}
//Geçici olarak her durumda swagger çalışacak.
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pandora.API v1");
});

//app.UseCors("AllowSpecificOrigins"); // Use the correct CORS policy here

app.UseCors("AllowAll"); // Use the correct CORS policy here

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PandoraDbContext>();
    dbContext.Database.Migrate();
}

// Uygulama başladıktan sonra URL'leri loglama
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()?.Addresses;
    if (addresses != null && addresses.Any())
    {
        Log.Information("Application is running on the following URLs: {Urls}", string.Join(", ", addresses));
    }
    else
    {
        Log.Information("Application started but no addresses were found");
    }
});

app.Run();
