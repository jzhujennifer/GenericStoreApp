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
    public class ProductSalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductSalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductSales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context!.ProductSale!.Include(p => p.Order).Include(p => p.Product);
            return View(await applicationDbContext.ToListAsync());
        } 

        public async Task<IActionResult> ShoppingCart()
        {

            var applicationDbContext = _context!.ProductSale!.Where(x=>x.OrderID == _context.Order!.FirstOrDefault(x=>x.Email == User.Identity!.Name)!.OrderID).Include(p => p.Order).Include(p => p.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductSales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductSale == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSale
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductSaleID == id);
            if (productSale == null)
            {
                return NotFound();
            }

            return View(productSale);
        }

        // GET: ProductSales/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID");
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "ID");
            return View();
        }

        // POST: ProductSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/productsales/{id}", Name = "AddToCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductSaleID,ProductID,OrderID,Quantity")] ProductSale productSale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", productSale.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "ID", productSale.ProductID);
            return View(productSale);
        }

        // GET: ProductSales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductSale == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSale.FindAsync(id);
            if (productSale == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", productSale.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "ID", productSale.ProductID);
            return View(productSale);
        }

        // POST: ProductSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductSaleID,ProductID,OrderID,Quantity")] ProductSale productSale)
        {
            if (id != productSale.ProductSaleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSaleExists(productSale.ProductSaleID))
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
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", productSale.OrderID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "ID", productSale.ProductID);
            return View(productSale);
        }

        // GET: ProductSales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductSale == null)
            {
                return NotFound();
            }

            var productSale = await _context.ProductSale
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductSaleID == id);
            if (productSale == null)
            {
                return NotFound();
            }

            return View(productSale);
        }

        // POST: ProductSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductSale == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductSale'  is null.");
            }
            var productSale = await _context.ProductSale.FindAsync(id);
            if (productSale != null)
            {
                _context.ProductSale.Remove(productSale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShoppingCart));
        }
        

        private bool ProductSaleExists(int id)
        {
          return (_context.ProductSale?.Any(e => e.ProductSaleID == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> AddToCart(int orderId, int productId)
        {
            var newProductSale = new ProductSale
            {
                ProductID = productId,
                OrderID = orderId,
                Quantity = 1,

            };
            _context.Add(newProductSale);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }


        public async Task<IActionResult> AddToCart_ShoppingCart(int orderId, int productId)
        {
            var newProductSale = new ProductSale
            {
                ProductID = productId,
                OrderID = orderId,
                Quantity = 1,

            };
            _context.Add(newProductSale);

            await _context.SaveChangesAsync();
            return RedirectToAction("ShoppingCart", "ProductSales");
        }

        public async Task<IActionResult> DecrementFromCart(int id)
        {
            var productSale = _context.ProductSale.FirstOrDefault(x => x.ProductSaleID == id);

            productSale.Quantity--;

            if (productSale.Quantity < 1)
            {

                return await DeleteConfirmed(id);
            }

            _context.ProductSale.Update(productSale);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ShoppingCart));
        }
    }
}
