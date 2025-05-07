
using Abstraction;
using Domain.Contracts;
using E_Commerce.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Shared.ErrorModels;
using StackExchange.Redis;
using System.Reflection;
using System.Reflection.Metadata;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container Services
            // Add services to the container.

            builder.Services.AddControllers();
			builder.Services.AddDbContext<StoredDBContext>(options =>
            {
                var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(ConnectionString);
            });

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDbIntializer, DbInitilizer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddAutoMapper(typeof(AssemblyReferences).Assembly); 

            builder.Services.AddScoped<IServicesManager, ServicesManager>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var Errors = context.ModelState.Where(m => m.Value.Errors.Any()).Select(m => new ValidationError()
                    {
                        Field = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                    });

                    var Response = new ValidationErrorToReturn()
                    {
                        ValidationErrors = Errors
                    };
					return new BadRequestObjectResult(Response);
				};
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionString"));
            });
			#endregion

			var app = builder.Build();

            await IntializeDbAsync(app);

            using var scope =  app.Services.CreateScope();
            var dbInitilizer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
            await dbInitilizer.IntializeAsync();

			#region Middlewares - Configure Pipelines
			// Configure the HTTP request pipeline.
            app.UseMiddleware<CustomExceptionMiddleware>();
			if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }


        public static async Task IntializeDbAsync(WebApplication app)
        {
			using var scope = app.Services.CreateScope();
			var dbInitilizer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
			await dbInitilizer.IntializeAsync();
		}
    }
}
