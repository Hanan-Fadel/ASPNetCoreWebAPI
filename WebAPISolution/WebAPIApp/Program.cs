using Microsoft.EntityFrameworkCore;
using WebAPIApp.Data;
using WebAPIApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inject our DbContext Class (WebAPIDbContext) into the Services collection
builder.Services.AddDbContext<WebAPIDbContext>(options =>
{
    //get the connection string from the configuration file appsettings.json using builder.Configuration.GetConnectionString
    options.UseSqlServer(builder.Configuration.GetConnectionString("WalksDbConn"));
});

//whenever I ask for the IRegionRepositlry, give me the implemenation for RegionRepository 
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

//Inject Automapper, when the applocation starts, the automapper will scan the assembly for the app and look
//for all profiles that we have and then use these maps to map data.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
