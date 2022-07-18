using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task12.Controllers;
using Task12.Models;
using Task12.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IFinanceTypeService, FinanceTypeService>();
builder.Services.AddScoped<IRecordFinanceService, RecordFinanceService>();

builder.Services.AddMvcCore(mvcOptions =>
{
    mvcOptions.EnableEndpointRouting = false;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMvc();


app.Run();
