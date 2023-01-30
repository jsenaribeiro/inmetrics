using System.Security.Principal;
using Desafio.Application;
using Desafio.Domain;
using Desafio.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseRabbitMQ("test");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
