using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ServiceDesk.Data.Context
{
    internal class PersistenceContextFactory : IDesignTimeDbContextFactory<PersistenceContext>
    {
        public PersistenceContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var options = new DbContextOptionsBuilder<PersistenceContext>()
                .UseNpgsql(config.GetConnectionString("Database"))
                .Options;

            return new PersistenceContext(options);
        }
    }
}
