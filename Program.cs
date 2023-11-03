using Clinic.Midleware;
using Clinic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
namespace Clinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<CustomExceptionFilter>();
            builder.Services.AddTransient<GlobalErrorHandler>();
            builder.Services.AddDbContext<ClinicContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("mvcConnection")));
            builder.Services.AddCors();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            var app = builder.Build();          
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseExceptionMiddlewear();
            app.UseAuthorization();
            app.UseCors("AllowOrigin");
            app.MapControllers();
            app.Run();
        }
    }
}