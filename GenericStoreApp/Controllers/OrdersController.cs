using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Data;
using GenericStoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

namespace GenericStoreApp.Controllers
{
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {

            return _context.Order != null
                ? View(await _context.Order.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Order'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Email,Sold")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }


            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,Email,Sold")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Order'  is null.");
            }

            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.OrderID == id)).GetValueOrDefault();
        }

        [Route("Orders/AddToCart/{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var previousOrder = _context.Order?.FirstOrDefault(x => x.Email == User.Identity!.Name);

            if (previousOrder != null)
            {
                // Check for productsale , increment quantity if existing, add new

                var productSale = _context.ProductSale?.FirstOrDefault(x =>
                    x.OrderID == previousOrder.OrderID && x.ProductID == productId);

                if (productSale != null)
                {
                    //existing
                    productSale.Quantity++;
                    _context.Update(productSale);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }

                _context.Add(new ProductSale
                {
                    //add new
                    ProductID = productId,
                    OrderID = previousOrder.OrderID,
                    Quantity = 1

                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");

            }

            var newOrder = new Order
            {
                Email = User.Identity.Name,
                Sold = false,

            };

            _context.Add(newOrder);

            await _context.SaveChangesAsync();
            //
            var orderID = _context.Order.FirstOrDefault(x => x.Email == User.Identity.Name).OrderID;
            return RedirectToAction("AddToCart", "ProductSales", new { OrderID = orderID, ProductId = productId });


        }

        [Route("Orders/AddToCart_ShoppingCart/{productId}")]
        public async Task<IActionResult> AddToCart_ShoppingCart(int productId)
        {
            {
                var previousOrder = _context.Order?.FirstOrDefault(x => x.Email == User.Identity!.Name);

                if (previousOrder != null)
                {
                    // Check for productsale , increment quantity if existing, add new

                    var productSale = _context.ProductSale?.FirstOrDefault(x =>
                        x.OrderID == previousOrder.OrderID && x.ProductID == productId);

                    if (productSale != null)
                    {
                        //existing
                        productSale.Quantity++;
                        _context.Update(productSale);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("ShoppingCart", "ProductSales");
                    }

                    _context.Add(new ProductSale
                    {
                        //add new
                        ProductID = productId,
                        OrderID = previousOrder.OrderID,
                        Quantity = 1

                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShoppingCart", "ProductSales");

                }

                var newOrder = new Order
                {
                    Email = User.Identity.Name,
                    Sold = false,

                };

                _context.Add(newOrder);

                await _context.SaveChangesAsync();
                //
                var orderID = _context.Order.FirstOrDefault(x => x.Email == User.Identity.Name).OrderID;
                return RedirectToAction("AddToCart_ShoppingCart", "ProductSales",
                    new { OrderID = orderID, ProductId = productId });


            }
        }
    }
}
