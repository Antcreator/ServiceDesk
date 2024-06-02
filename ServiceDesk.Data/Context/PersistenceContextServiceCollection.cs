using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDesk.Data.Context
{
    public static class PersistenceContextServiceCollection
    {
        public static IServiceCollection AddPersistenceContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PersistenceContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
