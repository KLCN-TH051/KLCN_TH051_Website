using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Repositories.Data
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // 🔹 Cấu hình connection string tại design-time
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-SSVQNLTA;Initial Catalog=TestMH3layer;User ID=sa;Password=123;TrustServerCertificate=True");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
