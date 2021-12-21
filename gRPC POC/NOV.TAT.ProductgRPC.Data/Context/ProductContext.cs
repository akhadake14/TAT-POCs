using Microsoft.EntityFrameworkCore;
using NOV.TAT.ProductgRPC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Data.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public ProductContext()
        {

        }
        public virtual DbSet<Product> Products { get; set; }
    }
}
