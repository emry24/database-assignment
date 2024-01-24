using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=DESKTOP-RA22D0F\MSSQLSERVER02;Initial Catalog=database1;Integrated Security=True;Trust Server Certificate=True"));
    services.AddScoped<UserService>();

    services.AddScoped<UserRepository>();
    services.AddScoped<AddressRepository>();
    services.AddScoped<AuthRepository>();
    services.AddScoped<ProfileRepository>();
    services.AddScoped<RoleRepository>();



    services.AddSingleton<MenuService>();

}).Build();


var menuService = builder.Services.GetRequiredService<MenuService>();
menuService.ShowMainMenu();
