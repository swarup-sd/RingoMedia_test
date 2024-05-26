using Microsoft.EntityFrameworkCore;
using RingoMedia_test.Data;
using RingoMedia_test.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), op =>
    {
        op.EnableRetryOnFailure();
    });

});

builder.Services.AddHostedService<ReminderEmailService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddHostedService<ReminderEmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Departments}/{action=Index}/{id?}");

app.Run();
