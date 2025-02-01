using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration); // tylko jeden parametr- configuration, bo mamy this i extendujemy już IServiceCollection
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200")); // allow any method czyt. get, post, delete itp.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
