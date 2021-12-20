using NOV.TAT.ProductgRPC.Data.Context;
using NOV.TAT.ProductgRPC.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Data
{
    public class ProductRepository : GenericRepository<Product, ProductContext>
    {
        public ProductRepository(ProductContext _context) : base(_context)
        {
        }
    }
}
