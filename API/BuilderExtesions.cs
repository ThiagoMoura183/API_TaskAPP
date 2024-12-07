using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infra.Persistency;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Application.Mappings;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
    }
}
