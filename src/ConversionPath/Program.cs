using AutoMapper;
using ConversionPath.Application.ExchangeRates.Commands;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.DomainModels.ExchangeRates;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Domain.ExchangeRates.Validation;
using ConversionPath.MappingProfile;
using ConversionPath.Persistence.Context;
using ConversionPath.Persistence.ExchangeRateAggregate.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.
var services = builder.Services;

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ExchangeRateDtoProfile()); 
});

IMapper mapper = mapperConfig.CreateMapper();
services.AddSingleton(mapper);

services.AddTransient<IValidator<ExchangeRate>, ExchangeRateValidator>();
services.AddSingleton<IDomainCollection<ExchangeRate>, ExchangeRateCollection>();
services.AddTransient<IRepositoryBase<ExchangeRate>, ExchangeRateRepository>();
services.AddMediatR(typeof(CreateExchangeRateCommand).GetTypeInfo().Assembly);

services.AddRazorPages();
services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase("ConversionPath");
    }
    else
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
    }
});

var app = builder.Build();

using (var serviceScope = app.Services?.GetService<IServiceScopeFactory>()?.CreateScope())
{
    if (serviceScope != null)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        if (builder.Environment.IsProduction())
        {
            context.Database.Migrate();
        }
    } 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
