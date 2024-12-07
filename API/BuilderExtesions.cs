using Application.UserCQ.Commands;
using Application.UserCQ.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Infra.Persistency;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace API {
    public static class BuilderExtesions {
        public static void AddServices(this WebApplicationBuilder builder) {

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
    }
}
