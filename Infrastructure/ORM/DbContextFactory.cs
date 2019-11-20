using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.ORM
{
    public class DbContextFactory: IDesignTimeDbContextFactory<CheckListDbContext>
    {
        public CheckListDbContext CreateDbContext(string[] args)
        {
            string connectionString = "server=localhost;userid=personal;password=personal;database=CheckListDB;persistsecurityinfo=True";
            var builder = new DbContextOptionsBuilder<CheckListDbContext>();
            builder.UseMySQL(connectionString);
            var dbContext = new CheckListDbContext(builder.Options, connectionString);
            return dbContext;
        }
    }
}
