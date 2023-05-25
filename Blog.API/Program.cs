using Blog.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnectionString")));
            builder.Services.AddCors(options => options.AddPolicy("default", policy =>
      {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
      }));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            { 
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("default");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
