﻿using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Business.DependencyResolver;
using Business.Utilities.Storage.Concrete.AwsStorage;
using Core.DependencyResolver;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();


// Konfiqurasiya sistemin? appsettings.json faylını ?lav? edir.
// - "optional: false": Fayl mütl?q mövcud olmalıdır. ?ks halda, t?tbiq x?ta ver?c?k.
// - "reloadOnChange: true": Faylın m?zmununda d?yişiklik olarsa, sistem yenil?nmiş parametrl?ri avtomatik yükl?y?c?k.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Add services to the container.
builder.Services.AddBusinessService();
builder.Services.AddCoreService();
builder.Services.AddStorageService<AwsStorage>();

builder.Services.AddCors(x=>x.AddPolicy("Policy",policyBuilder=>
{

policyBuilder
    .WithOrigins("https://turbo.az/", "https://localhost:3000/")
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed((host) => true);
}));


#region API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("Api-Version"),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
#endregion

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        x.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = $" WebApi {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        });
    }
    //x.SwaggerDoc("v", new OpenApiInfo { Title = "N layer Arch", Version = "v", Description = "N layer Arch swagger client." });
    //x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    x.SwaggerDoc("v", new OpenApiInfo { Title = "WebApi", Version = "v", Description = "Identity Service API swagger client." });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\\"  
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            Array.Empty<string>()
        }

    });
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            expires != null ? expires > DateTime.UtcNow : false,
        //ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Email
    };
});
//builder.Services.AddSwaggerGen();
    
//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
FluentValidationMvcExtensions.AddFluentValidation(builder.Services.AddControllersWithViews(), x =>
{
    x.RegisterValidatorsFromAssemblyContaining<Program>();
    x.ValidatorOptions.LanguageManager.Culture = new System.Globalization.CultureInfo("az");
});

builder.Services.AddLogging();

builder.Host.UseSerilog((context, loggerInformation) =>
{
    loggerInformation.ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddTransient<GlobalHandlingExceptionMiddleware>();
builder.Services.AddTransient<LocalizationMiddleware>();


var app = builder.Build();  

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {

     foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    }
    }
    );
}

app.UseCors("Policy");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalHandlingExceptionMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();

app.MapControllers();

app.Run();
