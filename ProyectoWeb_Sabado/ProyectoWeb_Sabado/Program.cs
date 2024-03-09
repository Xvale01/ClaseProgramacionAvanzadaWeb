using ProyectoWeb_Sabado.Models;
using ProyectoWeb_Sabado.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IUsuarioModel, UsuarioModel>();
builder.Services.AddSingleton<IUtilitariosModel, UtilitariosModel>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=IniciarSesion}/{id?}");

app.Run();
