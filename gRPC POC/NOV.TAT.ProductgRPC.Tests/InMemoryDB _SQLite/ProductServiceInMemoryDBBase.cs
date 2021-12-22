using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NOV.TAT.ProductgRPC.Business.Models;
using NOV.TAT.ProductgRPC.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Tests.InMemoryDB__SQLite
{
    public  class ProductServiceInMemoryDBBase : IDisposable
    {
        public ProductContext context { get; set; }

        public ProductServiceInMemoryDBBase()
        {
            context = CreateContextForInMemory();
        }
        public ProductContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;

            context = new ProductContext(option);
            Seed(context);
            return context;
        }

        public ProductContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var option = new DbContextOptionsBuilder<ProductContext>().UseSqlite(connection).Options;

            context = new ProductContext(option);
            Seed(context);
            return context;

        }

        private void Seed(ProductContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var data = new List<Product>
            {
                new Product {Id=1, Name = "Product1" ,Description="Description1", UnitPrice=11.11f},
                 new Product {Id=2, Name = "Product2" ,Description="Description2", UnitPrice=21.11f},
                 new Product {Id=3, Name = "Product3" ,Description="Description3", UnitPrice=31.11f},
            }.AsQueryable();

            context.Products.AddRange(data);
            context.SaveChanges();
        }

        public void Dispose()
        {
           context.Dispose();
        }        
    }
}
