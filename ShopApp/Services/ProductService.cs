using System.Collections.Generic;
using ShopDomain.Models;

using ShopApp.Interfaces;

namespace ShopApp.Services
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
