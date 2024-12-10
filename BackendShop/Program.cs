using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data.Data;
using BackendShop.Data.DataSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IImageHulk, ImageHulk>();

var app = builder.Build();
app.UseCors(opt =>
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

string imagesDirPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"]);

if (!Directory.Exists(imagesDirPath))
{
    Directory.CreateDirectory(imagesDirPath);
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesDirPath),
    RequestPath = "/images"
});

app.UseAuthorization();
app.SeedDataAsync();

app.MapControllers();

app.Run();
