using AutoMapper;
using Grpc.Core;
using Grpc.Core.Testing;
using Grpc.Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NOV.TAT.ProductgRPC.Business.Models;
using NOV.TAT.ProductgRPC.Data;
using NOV.TAT.ProductgRPC.Data.Context;
using NOV.TAT.ProductgRPC.Service.Mapper;
using NOV.TAT.ProductgRPC.Service.Services;
using ProductGRPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NOV.TAT.ProductgRPC.Tests
{
    [TestClass]
    public class ProductServiceTest
    {
        private ProductService productservice;
        private IMapper mapper;
        private ILogger<ProductService> logger;

        private Mock<DbSet<Product>> mockSet;
        private Mock<ProductContext> mockContext;
        private ServerCallContext serverCallContext;

        public ProductServiceTest()
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

            mockSet = new Mock<DbSet<Product>>();
            mockContext = new Mock<ProductContext>();

            serverCallContext=GetTestServerCallContext();

        }

        [TestMethod]
        public void AddProductTest()
        {
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            productservice = new ProductService(new ProductRepository(mockContext.Object), mapper, logger);

            var productResponse = productservice.AddProduct(new ProductRequest()
            {
                Product=new ProductModel()
                {
                    Id=5,
                    Name="Test",
                    Description= "DescriptionTest",
                    UnitPrice=55.8f
                }
            }, serverCallContext);

            Assert.AreEqual((int)ResponseStatusCode.Ok, productResponse.Result.StatusCode);
            Assert.AreEqual(ResponseStatusCode.Created.ToString(), productResponse.Result.Description);
        }

        [TestMethod]
        public void GetAllProductTest()
        {
            var data = new List<Product>
            {
                new Product {Id=1, Name = "Product1" ,Description="Description1", UnitPrice=11.11f},
                 new Product {Id=2, Name = "Product2" ,Description="Description2", UnitPrice=21.11f},
                 new Product {Id=3, Name = "Product3" ,Description="Description3", UnitPrice=31.11f},
            }.AsQueryable();

            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            productservice = new ProductService(new ProductRepository(mockContext.Object), mapper, logger);

            var productResponse = productservice.GetProduts(new ProductRequest(), serverCallContext);

            Assert.AreEqual(3, productResponse.Result.Products.Count);
            Assert.AreEqual(1, productResponse.Result.Products[0].Id);
            Assert.AreEqual("Product2", productResponse.Result.Products[1].Name);
            Assert.AreEqual("Description3", productResponse.Result.Products[2].Description);
        }

        [TestMethod]
        public void GetbyIDTest()
        {
            var data = new List<Product>
            {
                new Product {Id=1, Name = "Product1" ,Description="Description1", UnitPrice=11.11f},
                 new Product {Id=2, Name = "Product2" ,Description="Description2", UnitPrice=21.11f},
                 new Product {Id=3, Name = "Product3" ,Description="Description3", UnitPrice=31.11f},
            }.AsQueryable();

            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            productservice = new ProductService(new ProductRepository(mockContext.Object), mapper, logger);

            var productResponse = productservice.Get(new ProductRequest() 
            { 
                Product = new ProductModel() 
                { 
                    Id=2
                }
            }, serverCallContext);

            Assert.AreEqual(2, productResponse.Result.Product.Id);
            Assert.AreEqual("Product2", productResponse.Result.Product.Name);
            Assert.AreEqual("Description2", productResponse.Result.Product.Description);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var data = new List<Product>
            {
                new Product {Id=1, Name = "Product1" ,Description="Description1", UnitPrice=11.11f},
                 new Product {Id=2, Name = "Product2" ,Description="Description2", UnitPrice=21.11f},
                 new Product {Id=3, Name = "Product3" ,Description="Description3", UnitPrice=31.11f},
            }.AsQueryable();

            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            productservice = new ProductService(new ProductRepository(mockContext.Object), mapper, logger);

            var productResponse = productservice.UpdateProduct(new ProductRequest()
            {
                Product = new ProductModel()
                {
                    Id = 2,
                    Name = "Updated Name",
                    Description = "Updated dis",
                    UnitPrice=787.5f
                }
            }, serverCallContext);

            Assert.AreEqual((int)ResponseStatusCode.Ok, productResponse.Result.StatusCode);
            Assert.AreEqual(ResponseStatusCode.Updated.ToString(), productResponse.Result.Description);
            Assert.AreEqual(2, productResponse.Result.Product.Id);
            Assert.AreEqual("Updated Name", productResponse.Result.Product.Name);
        }


        [TestMethod]
        public void DeleteTest()
        {
            var data = new List<Product>
            {
                new Product {Id=1, Name = "Product1" ,Description="Description1", UnitPrice=11.11f},
                 new Product {Id=2, Name = "Product2" ,Description="Description2", UnitPrice=21.11f},
                 new Product {Id=3, Name = "Product3" ,Description="Description3", UnitPrice=31.11f},
            }.AsQueryable();

            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());


            mockContext = new Mock<ProductContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            productservice = new ProductService(new ProductRepository(mockContext.Object), mapper, logger);

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