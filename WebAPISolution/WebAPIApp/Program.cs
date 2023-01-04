using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPIApp.Data;
using WebAPIApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure the Swagger to add authentication (i.e. configure swagger to make token calls)
builder.Services.AddSwaggerGen(options=>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter a valid JWT bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

//Add Fluent Validation 
#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddFluentValidation(options =>
       options.RegisterValidatorsFromAssemblyContaining<Program>());
#pragma warning restore CS0618 // Type or member is obsolete

//Inject our DbContext Class (WebAPIDbContext) into the Services collection
builder.Services.AddDbContext<WebAPIDbContext>(options =>
{
    //get the connection string from the configuration file appsettings.json using builder.Configuration.GetConnectionString
    options.UseSqlServer(builder.Configuration.GetConnectionString("WalksDbConn"));
});

//whenever I ask for the IRegionRepositlry, give me the implemenation for RegionRepository 
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

//Add the WalkRepository so that it can be used by the controller
builder.Services.AddScoped<IWalkRepository, WalkRepository>();

//Add the WalkDifficultyRepository so that it can be used inside the WalkDifficultyController
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//Add the StaticUserRepository, here we will use AddSingelton because we are using the static repository so we need only instance of the static user repository 
//and therefore no new Guid would be generated
/*builder.Services.AddSingleton<IUserRepository, StaticUserRepository>();*/

//Add the UserRepository, 
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Add Token Handler
builder.Services.AddScoped<ITokenHandler, WebAPIApp.Repositories.TokenHandler>();


//Inject Automapper, when the applocation starts, the automapper will scan the assembly for the app and look
//for all profiles that we have and then use these maps to map data.
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//Inject Authentication 
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    options.TokenValidationParameters= new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer= builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//To use the authentication inside the middleware of AspNetcore, add this line before app.UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
