using GenericStoreApp.Data;
using GenericStoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace GenericStoreApp.Services
{
    public class OrderService
    {

        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal?> GetPriceForProductSale(int productSaleId)
        {
            
            var productSale =  _context!.ProductSale!.FirstOrDefault(x => x.ProductSaleID == productSaleId);
            var quantity = productSale.Quantity;
            var price = _context!.Product!.FirstOrDefault(x => x.ID == productSale.ProductID).Price;

            return price * quantity;

        }

        public async Task<decimal?> GetPriceForOrder(int orderID)
        {
            decimal? totalPrice = 0;
         

            var productSales = _context!.ProductSale!.Where(x => x.OrderID == orderID);
            

            foreach (var sale in productSales)
            {
                totalPrice += _context!.Product!.FirstOrDefault(x => x.ID == sale.ProductID)?.Price * sale.Quantity;

            }

            return totalPrice;
        }

        public async Task AddToCart(int productId)
        {

            

            await _context.SaveChangesAsync();
        }


    }


}
