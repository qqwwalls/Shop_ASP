using System.Collections.Generic;
using ShopDomain.Models;

using ShopApplication.Interfaces;

namespace ShopApplication.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> _products = new List<Product>();

        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
