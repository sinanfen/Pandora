using Microsoft.EntityFrameworkCore;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Application.Extensions;
using Pandora.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register services from different layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddCoreServices();
//builder.Services.AddCrossCuttingConcernsServices();

builder.Services.AddDbContext<PandoraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PandoraBoxDatabase")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
