using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using ProductGRPCService;

// The port number must match the port of the gRPC server.

var credentials = CallCredentials.FromInterceptor((context, metadata) =>
{
    string? _token = "TestKey";
    if (!string.IsNullOrEmpty(_token))
    {
        metadata.Add("Authorization", $"Bearer {_token}");
    }
    return Task.CompletedTask;
});

var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
});

//var channel = GrpcChannel.ForAddress("http://localhost:5186", new GrpcChannelOptions() { });






var client = new ProductGRPCService.ProductGrpc.ProductGrpcClient(channel);


//var createCallReply = await client.AddProductAsync(new ProductRequest()
//{
//    Product = new ProductModel()
//    {
//        Name = "Product10",
//        Description = " Description10",
//        UnitPrice = 100.24f
//    }
//});

//Console.WriteLine($"Create Call : {createCallReply.Product}");

//Console.WriteLine("-----------------------------------------------------------------------------------");


var getCallReplay = await client.GetProdutsAsync(new ProductRequest());
Console.WriteLine($"Get all Products  Call :{getCallReplay} ");
Console.WriteLine("-----------------------------------------------------------------------------------");
Console.WriteLine("Get Call : " + await client.GetAsync(new ProductRequest()
{
    Product = new ProductModel() { Id = 5 }
}));
Console.WriteLine("-----------------------------------------------------------------------------------");


var updateReplay = await client.UpdateProductAsync(new ProductRequest()
{
    Product = new ProductModel()
    {
        Id = 4,
        Name = "Product6",
        Description = " Description6",
        UnitPrice = 20.24f
    }
});
Console.WriteLine($"Update Call : {updateReplay}");
Console.WriteLine("-----------------------------------------------------------------------------------");



Console.WriteLine("Delete Call : " + await client.DeleteProductAsync(new ProductRequest()
{
    Product = new ProductModel()
    {
        Id = 7,
        Name = "Product6",
        Description = " Description6",
        UnitPrice = 20.24f
    }
}));
Console.WriteLine("-----------------------------------------------------------------------------------");


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
