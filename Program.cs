
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Xml;
using EmployeeAccess.Model;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        try
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        }
        catch (Exception ex)
        {
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    EmployeeAccess.Model.ValidationSummary validationSummary = new EmployeeAccess.Model.ValidationSummary { IsValid = true, Messages = new List<ValidationMessage>() };
                    validationSummary.Messages.Add(new ValidationMessage { Type = EmployeeAccess.Model.ValidationType.INVALID, Message = ex.Message });

                    var result = new
                    {
                        Result = "INVALID",
                        Messages = validationSummary.Messages
                    };

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                    context.HandleResponse();
                },
                OnForbidden = async context =>
                {
                    EmployeeAccess.Model.ValidationSummary validationSummary = new EmployeeAccess.Model.ValidationSummary { IsValid = true, Messages = new List<ValidationMessage>() };
                    validationSummary.Messages.Add(new EmployeeAccess.Model.ValidationMessage { Type = EmployeeAccess.Model.ValidationType.INVALID, Message = "Forbidden API access" });

                    var result = new
                    {
                       Result = "INVALID",
                        Messages = validationSummary.Messages
                    };

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            };
        

        // Ensure options.Events are set even if there are no exceptions
        if (options.Events == null)
        {
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    EmployeeAccess.Model.ValidationSummary validationSummary = new EmployeeAccess.Model.ValidationSummary { IsValid = true, Messages = new List<ValidationMessage>() };
                    validationSummary.Messages.Add(new EmployeeAccess.Model.ValidationMessage { Type = EmployeeAccess.Model.ValidationType.INVALID, Message = "Unauthorized API access" });

                    var result = new
                    {
                        Result = "INVALID",
                        Messages = validationSummary.Messages
                    };

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                    context.HandleResponse();
                },
                OnForbidden = async context =>
                {
                    EmployeeAccess.Model.ValidationSummary validationSummary = new EmployeeAccess.Model.ValidationSummary { IsValid = true, Messages = new List<ValidationMessage>() };
                    validationSummary.Messages.Add(new EmployeeAccess.Model.ValidationMessage { Type = EmployeeAccess.Model.ValidationType.INVALID, Message = "Forbidden API access" });

                    var result = new
                    {
                        Result = "INVALID",
                        Messages = validationSummary.Messages
                    };

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            };
        }
    }

    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configure JWT authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \n\r\n Enter 'Bearer' [space] and then your token in the text input below. \n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Set SQL connection configuration
ConfigurationManager configuration = builder.Configuration;
builder.Configuration.AddJsonFile($"appsettings.Development.json");
// Configure the HTTP request pipeline.

//Console.WriteLine("MY ENV: " + builder.Configuration.GetConnectionString("myenv"));
//Console.WriteLine("BE_URL: " + builder.Configuration.GetConnectionString("BE_URL"));


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
