using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Application.Mapping;
using BillingInvoicingPlatform.Application.Service;
using BillingInvoicingPlatform.Application.Validators;
using BillingInvoicingPlatform.Infrastructure.Data;
using BillingInvoicingPlatform.Infrastructure.Data.Seed;
using BillingInvoicingPlatform.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure DbContext with SQL Server:
builder.Services.AddDbContext<ApplicationDbContext>(options=>options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add auto-mapper configuration:

builder.Services.AddAutoMapper(cfg => { }, typeof(CustomerProfile).Assembly);


// Register validators from assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();


// Register Repositories:
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();


//Register Services:

builder.Services.AddScoped<CustomerService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Seed Data =====
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    // Optional: apply migrations automatically
//  //  dbContext.Database.Migrate();

//    await CustomerSeeder.SeedAsync(dbContext);
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
