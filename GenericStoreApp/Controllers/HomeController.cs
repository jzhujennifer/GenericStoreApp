using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenericStoreApp.Data;
using GenericStoreApp.Models;
using System.Diagnostics;

namespace GenericStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var products = _context.Product.ToList();
            return View(products);
        }

        [Route("home/sort/{sortCategory}")]
        public async Task<IActionResult> Index(string sortCategory)
        {

            var sortedProducts = new List<Product>();
            if (_context.Product != null)
            {
                var products = await _context.Product.ToListAsync();
                sortedProducts = products.FindAll(p => p.Category == sortCategory);

            }


            return _context.Product != null ?
                View(sortedProducts) :
                Problem("Entity set 'ApplicationDbContext.Product'  is null.");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        // GET: ContactUs/Create
        public IActionResult Contact()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,Name,Email,Message")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                TempData["message"] = "message sent successfully!";
/*                return RedirectToAction(nameof(Index));
*/            }
            return View(contactUs);
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

    }
}