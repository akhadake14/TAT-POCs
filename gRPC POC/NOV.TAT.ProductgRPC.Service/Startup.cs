﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using NOV.TAT.ProductgRPC.Data;
using NOV.TAT.ProductgRPC.Data.Context;
using NOV.TAT.ProductgRPC.Data.Models;
using System.Configuration;

namespace NOV.TAT.ProductgRPC.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddAutoMapper(typeof(Startup)); //AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddScoped<GenericRepository<Product, ProductContext>, ProductRepository>();
            services.AddDbContext<ProductContext>(options =>
                options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductDB;Integrated Security=True;Pooling=False",
                    b => b.MigrationsAssembly(typeof(ProductContext).Assembly.FullName)));

            services.AddGrpcReflection();  //Grpc.AspNetCore.Server.Reflection
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.ProductService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }
            });
        }
    }
}
