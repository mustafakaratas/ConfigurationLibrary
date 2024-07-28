using ConfigurationApi.Data;
using ConfigurationApi.V1.Models.Requests.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateConfigurationRequestValidator>();

builder.Services.AddDbContext<ConfigurationDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("ConfigurationDb"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();