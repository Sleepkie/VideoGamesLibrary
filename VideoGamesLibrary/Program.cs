
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VideoGamesLibrary.DbContexts;
using VideoGamesLibrary.Interfaces;
using VideoGamesLibrary.UnitsOfWork;

namespace VideoGamesLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(); 

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<VideoGamesLibraryContext>(options => options.UseNpgsql(connection));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}