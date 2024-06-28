using Microsoft.EntityFrameworkCore;
using linkApi.DataContext;
using linkApi.Repositories;
using linkApi.Interfaces;
using linkApi.Services;
using linkApi.Factories;

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


// Add CORS policy

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*")
            .WithMethods("POST", "DELETE", "GET")
            .AllowAnyHeader();
        });
});

// Add services to the container.

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<LinkShortenerContext>(options =>
    options.UseNpgsql(connectionString),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Transient
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUrlRepo, UrlRepo>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IUrlFactory, UrlFactory>();

/* ############################################################## */ 

var app = builder.Build();

// ure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.Map("/", () => { return "index"; });

app.Run();
