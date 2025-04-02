using API.Data;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions // static pozwala do używania metod w klasie bez koniecznosci tworzenia nowych instancji 
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        // Add.Singleton - zawsze używa tej samej instacji, dobre np. kiedy chcemy utrzymać jakiś stan cały czas, AddTransient - za każdym razem nowa, dla lekkich usług, AddScoped - tworzone sa na żądanie klienta
        services.AddScoped<ITokenService, TokenService>(); // może być samo TokenService, ale używa się abstarkcji interfejsu
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<LogUserActivity>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        return services;
    }
}
