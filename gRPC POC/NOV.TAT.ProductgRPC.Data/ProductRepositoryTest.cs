using NOV.TAT.ProductgRPC.Data.Models;

namespace NOV.TAT.ProductgRPC.Data
{
    public class ProductRepositoryTest : IRepository<Product>
    {
        /* private ProductContext _productContext;
         public ProductRepository(ProductContext productContext)
         {
             this._dataContext = productContext;
         }
        */

        List<Product> _products = new List<Product>();
        public ProductRepositoryTest()
        {
            _products.Add(new Product()
            {
                Id = 1,
                Name = "Product1",
                Description = " Description1",
                UnitPrice = 10.24f
            });
            _products.Add(new Product()
            {
                Id = 2,
                Name = "Product2",
                Description = " Description2",
                UnitPrice = 20.24f
            });
            _products.Add(new Product()
            {
                Id = 3,
                Name = "Product3",
                Description = " Description3",
                UnitPrice = 20.24f
            });
            _products.Add(new Product()
            {
                Id = 4,
                Name = "Product4",
                Description = " Description4",
                UnitPrice = 20.24f
            });
            _products.Add(new Product()
            {
                Id = 5,
                Name = "Product5",
                Description = " Description5",
                UnitPrice = 20.24f
            });
        }
        public Product? GetById(int id)
        {
            return _products != null ?
                _products.SingleOrDefault(product => product.Id == id)
                : null;
        }
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public void Insert(Product product)
        {
            _products.Add(product);
        }

        public void Update(Product product)
        {
            if (GetById(product.Id) is Product product1)
                product1.Name = product.Name;


        }
        public void Delete(int id)
        {
            if (GetById(id) is Product product)
                _products.Remove(product);
        }
        public void Save()
        {
            throw new NotImplementedException();
        }

    }
}
