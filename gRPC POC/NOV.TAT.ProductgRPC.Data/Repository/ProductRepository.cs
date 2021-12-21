using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using NOV.TAT.ProductgRPC.Business.Models;
using NOV.TAT.ProductgRPC.Data.Context;
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

        public override IEnumerable<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public override Product? GetById(int id)
        {
            return context.Products.FirstOrDefault(x => x.Id == id);
        }

        public override void Insert(Product product)
        {
            context.Products.Add(product);
        }

        public override void Delete(int id)
        {
            Product product = GetById(id);
            if (product != null)
                context.Products.Remove(product);
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));
        }

        public override void Update(Product product)
        {
            Product dbproduct = GetById(product.Id);
            if (product != null && dbproduct != null)
            {
                dbproduct.Name = product.Name;
                dbproduct.Description = product.Description;    
                dbproduct.UnitPrice = product.UnitPrice;
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));
        }
    }
}
