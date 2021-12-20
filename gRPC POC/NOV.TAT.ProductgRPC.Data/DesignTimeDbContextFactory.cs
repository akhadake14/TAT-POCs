using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NOV.TAT.ProductgRPC.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../NOV.TAT.ProductgRPC.Service/appsettings.json").Build(); //C:\Personal Drive\Code\TAT-Project Practice\GRPC_POC\NOV.TAT.ProductgRPC.Service\appsettings.json
            var builder = new DbContextOptionsBuilder<ProductContext>();
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductDB;Integrated Security=True";//configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new ProductContext(builder.Options);
        }
    }
}
