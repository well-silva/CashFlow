using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependecyInjectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddDbContext(services, configuration);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            var versionDb = new Version(8, 2, 0);
            var serverVersion = new MySqlServerVersion(versionDb);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
