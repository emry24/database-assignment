using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=DESKTOP-RA22D0F\MSSQLSERVER02;Initial Catalog=database1;Integrated Security=True;Trust Server Certificate=True"));

}).Build();
