﻿using EcommerceHao.Data;
using EcommerceHao.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HaoLamShopContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("HaoLamShop"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/KhachHang/DangNhap";
    options.AccessDeniedPath = "/AccessDenied";
});
// đăng ký PaypalClient dạng Singleton() - chỉ có 1 instance duy nhất trong toàn ứng dụng
builder.Services.AddSingleton(x => new PaypalClient(
        builder.Configuration["PaypalOptions:AppId"],
        builder.Configuration["PaypalOptions:AppSecret"],
        builder.Configuration["PaypalOptions:Mode"]
));

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

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
