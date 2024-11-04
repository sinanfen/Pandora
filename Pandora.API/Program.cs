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
using Pandora.Application.Interfaces;
using Microsoft.OpenApi.Models;

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
        ClockSkew = TimeSpan.Zero // Token s�resi tam doldu�unda ge�ersiz olur
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pandora.API", Version = "v1" });

    // JWT Bearer token i�in Swagger ayarlar�
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins("https://localhost:7192") //4200 Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()); // Required if you're using authentication cookies or tokens
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin"); // Use the correct CORS policy here
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
