using AutoMapper;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NOV.TAT.ProductgRPC.Data;
using NOV.TAT.ProductgRPC.Service.Mapper;
using NOV.TAT.ProductgRPC.Service.Services;
using ProductGRPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NOV.TAT.ProductgRPC.Tests.InMemoryDB__SQLite
{
    [TestClass]
    public class ProductServiceSQLiteDBTests :ProductServiceInMemoryDBBase
    {
        private ProductService productservice;
        private IMapper mapper;
        private ILogger<ProductService> logger;
        private ServerCallContext serverCallContext;
        public ProductServiceSQLiteDBTests() : base()
        {

        }

        [TestInitialize]
        public void TestInit()
        {
            var myProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);

            var mockLogger = new Mock<ILogger<ProductService>>();
            logger = mockLogger.Object;
            serverCallContext = GetTestServerCallContext();

        }

        [TestMethod]
        public void AddProductTest()
        {
            using (context = CreateContextForSQLite())
            {
                productservice = new ProductService(new ProductRepository(context), mapper, logger);

                var productResponse = productservice.AddProduct(new ProductRequest()
                {
                    Product = new ProductModel()
                    {
                        Id = 5,
                        Name = "Test",
                        Description = "DescriptionTest",
                        UnitPrice = 55.8f
                    }
                }, serverCallContext);

                Assert.AreEqual((int)ResponseStatusCode.Ok, productResponse.Result.StatusCode);
                Assert.AreEqual(ResponseStatusCode.Created.ToString(), productResponse.Result.Description);
            }
        }
        [TestMethod]
        public void GetAllProductTest()
        {
            using (context = CreateContextForSQLite())
            {
                productservice = new ProductService(new ProductRepository(context), mapper, logger);

                var productResponse = productservice.GetProduts(new ProductRequest(), serverCallContext);

                Assert.AreEqual(3, productResponse.Result.Products.Count);
                Assert.AreEqual(1, productResponse.Result.Products[0].Id);
                Assert.AreEqual("Product2", productResponse.Result.Products[1].Name);
                Assert.AreEqual("Description3", productResponse.Result.Products[2].Description);
            }
        }

        [TestMethod]
        public void GetbyIDTest()
        {
            using (context = CreateContextForSQLite())
            {
                productservice = new ProductService(new ProductRepository(context), mapper, logger);

                var productResponse = productservice.Get(new ProductRequest()
                {
                    Product = new ProductModel()
                    {
                        Id = 2
                    }
                }, serverCallContext);

                Assert.AreEqual(2, productResponse.Result.Product.Id);
                Assert.AreEqual("Product2", productResponse.Result.Product.Name);
                Assert.AreEqual("Description2", productResponse.Result.Product.Description);
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            using (context = CreateContextForSQLite())
            {
                productservice = new ProductService(new ProductRepository(context), mapper, logger);

                var productResponse = productservice.UpdateProduct(new ProductRequest()
                {
                    Product = new ProductModel()
                    {
                        Id = 2,
                        Name = "Updated Name",
                        Description = "Updated dis",
                        UnitPrice = 787.5f
                    }
                }, serverCallContext);

                Assert.AreEqual((int)ResponseStatusCode.Ok, productResponse.Result.StatusCode);
                Assert.AreEqual(ResponseStatusCode.Updated.ToString(), productResponse.Result.Description);
                Assert.AreEqual(2, productResponse.Result.Product.Id);
                Assert.AreEqual("Updated Name", productResponse.Result.Product.Name);
            }
        }


        [TestMethod]
        public void DeleteTest()
        {
            using (context = CreateContextForSQLite())
            {

                productservice = new ProductService(new ProductRepository(context), mapper, logger);

                var productResponse = productservice.DeleteProduct(new ProductRequest()
                {
                    Product = new ProductModel()
                    {
                        Id = 2
                    }
                }, serverCallContext);

                Assert.AreEqual((int)ResponseStatusCode.Ok, productResponse.Result.StatusCode);
                Assert.AreEqual(ResponseStatusCode.Deleted.ToString(), productResponse.Result.Description);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #region Private methods 
        private ServerCallContext GetTestServerCallContext()
        {
            return TestServerCallContext.Create
                            ("", null, DateTime.UtcNow.AddHours(1), new Metadata(), CancellationToken.None, "", null, null,
                            (metadata) => TaskUtils.CompletedTask, () => new WriteOptions(), (writeOptions) => { });
        }

        #endregion
    }
}
