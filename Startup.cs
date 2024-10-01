using EmployeeAccess.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EmployeeAccess
{
    public class StartUp
    {
        private readonly IConfiguration configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
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
                                        // Result = HttpResult.INVALID,
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
                                        //Result = HttpResult.INVALID,
                                        Messages = validationSummary.Messages
                                    };

                                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                                }
                            };
                        }
                    }

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
                                    // Result = HttpResult.INVALID,
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
                                    //Result = HttpResult.INVALID,
                                    Messages = validationSummary.Messages
                                };

                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                            }
                        };
                    }
                   
                });

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                      $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Access", Version = "v1" });

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

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
