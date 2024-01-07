using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbRecipiesContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("sqlCoonection")));

builder.Services.AddCors(option => option.AddPolicy("myRule", 
    options => options.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()));

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("myRule");
app.MapControllers();

app.Run();
