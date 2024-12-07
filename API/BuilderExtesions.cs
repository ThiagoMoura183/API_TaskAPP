﻿using Application.Mappings;
using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using Domain.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra.Persistency;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.AuthService;
using System.Reflection;
using System.Text;


namespace API {
    public static class BuilderExtesions {

        public static void AddSwaggerDocs(this WebApplicationBuilder builder) {
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Tasks App API",
                    Version = "v1",
                    Description = "Um aplicativo de tarefas baseado no Trello e escrito em ASP.NET Core V8",
                    Contact = new OpenApiContact {
                        Name = "Exemplo de página de contato",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense {
                        Name = "Exemplo de página de licença",
                        Url = new Uri("https://example.com/license")
                    }
                });

                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }

        public static void AddJWTAuth(this WebApplicationBuilder builder) {
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            });
        }

        public static void AddServices(this WebApplicationBuilder builder) {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));
        }

        public static void AddDatabase(this WebApplicationBuilder builder) {
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<TasksDbContext>(options => options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!));
        }

        public static void AddValidations(this WebApplicationBuilder builder) {
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddFluentValidationAutoValidation();
        }

        public static void AddMapper(this WebApplicationBuilder builder) {
            builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
        }

        public static void AddInjections(this WebApplicationBuilder builder) {
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
    }
}
