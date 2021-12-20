using AutoMapper;
using Grpc.Core;
using NOV.TAT.ProductgRPC.Business.Models;
using NOV.TAT.ProductgRPC.Data;
using NOV.TAT.ProductgRPC.Data.Context;
using ProductGRPCService;
namespace NOV.TAT.ProductgRPC.Service.Services
{
    public class ProductService : ProductGrpc.ProductGrpcBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly GenericRepository<Product,ProductContext> _productRepository;
        private readonly IMapper _mapper; //Automapper



        public ProductService(GenericRepository<Product, ProductContext> productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region CURD Operations
        public override Task<ProductResponse> AddProduct(ProductRequest request, ServerCallContext context)
        {
            if (request != null && request.Product != null)
            {
                _logger.Log(LogLevel.Information, "This is Create Product Call ");
                _productRepository.Insert(_mapper.Map<Product>(request.Product));
                _productRepository.Save();
                _logger.Log(LogLevel.Information, "Create Product Call completed");
                return CreateResponse(request.Product, ((int)ResponseStatusCode.Ok), ResponseStatusCode.Created.ToString());
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));
        }

        public override Task<ProductResponse> Get(ProductRequest request, ServerCallContext context)
        {
            if (request != null && request.Product != null && request.Product.Id > 0)
            {
                _logger.Log(LogLevel.Information, "This is Get Call ");
                var product = _productRepository.GetById(request.Product.Id);
                if (product == null)
                    throw new RpcException(new Status(StatusCode.NotFound, $"Product is not found."));
                var productModel = _mapper.Map<ProductModel>(product);
                _logger.Log(LogLevel.Information, "Get Call completed ");
                return CreateResponse(productModel, ((int)ResponseStatusCode.Ok), ResponseStatusCode.Ok.ToString());
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));

        }
        public override Task<ProductResponse> GetProduts(ProductRequest request, ServerCallContext context)
        {
            if (request != null)
            {
                _logger.Log(LogLevel.Information, "This is Get All Product Call ");
                var products = _productRepository.GetAll();
                var productModels = _mapper.Map<List<ProductModel>>(products);
                _logger.Log(LogLevel.Information, "Get All Call completed ");
                return CreateResponse(productModels, ((int)ResponseStatusCode.Ok));
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));

        }
        public override Task<ProductResponse> UpdateProduct(ProductRequest request, ServerCallContext context)
        {
            if (request != null && request.Product != null)
            {
                _logger.Log(LogLevel.Information, "This is Update Product Call ");
                _productRepository.Update(_mapper.Map<Product>(request.Product));
                _productRepository.Save();
                _logger.Log(LogLevel.Information, "Update All Call completed ");
                return CreateResponse(request.Product, ((int)ResponseStatusCode.Ok), ResponseStatusCode.Updated.ToString());
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));
        }
        public override Task<ProductResponse> DeleteProduct(ProductRequest request, ServerCallContext context)
        {
            if (request != null && request.Product != null && request.Product.Id > 0)
            {
                _logger.Log(LogLevel.Information, "This is Delete Product Call ");
                _productRepository.Delete(request.Product.Id);
                _productRepository.Save();
                _logger.Log(LogLevel.Information, "Update All Call completed ");
                return CreateResponse(request.Product, ((int)ResponseStatusCode.Ok), ResponseStatusCode.Deleted.ToString());
            }
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Values can not be null"));
        }

        #endregion

        #region Private Methods
        private Task<ProductResponse> CreateResponse(ProductModel product, int statusCode, string description)
        {
            return Task.FromResult(new ProductResponse()
            {
                StatusCode =statusCode,
                Status ="",
                Description = description,
                Product = product

            });
        }
        private Task<ProductResponse> CreateResponse(List<ProductModel> products, int statusCode)
        {
            var response = new ProductResponse();
            response.StatusCode = statusCode;
            response.Products.AddRange(products);
            return Task.FromResult(response);
        }

        private Task<ProductResponse> CreateResponse(int statusCode , Error error)
        {
            return Task.FromResult(new ProductResponse()
            {
                StatusCode = statusCode,
                Error = error
            });
        }
        #endregion

    }
}
