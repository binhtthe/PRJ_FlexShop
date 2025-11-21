using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PRJ_MKS_BTT.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Configure your DbContext options here, e.g., connection string
            optionsBuilder.UseSqlServer("Server=DESKTOP-IB7BIAP;Database=PRJFlexShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            return new ApplicationDbContext(optionsBuilder.Options);
        }

    }
    
    
}
