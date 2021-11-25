using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreContext _storeContext;

        public HomeController(ILogger<HomeController> logger, StoreContext storeContext) // injection
        {
            _logger = logger;
            _storeContext = storeContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _storeContext.Products.ToListAsync();
            if (Request.Query.ContainsKey("isUpdate")) {
                var isUpdate = bool.Parse(Request.Query["isUpdate"]);
                ViewBag.IsUpdate = isUpdate;
            }
            ViewBag.Products = products;
            return View();
        }

        public IActionResult InsertProduct(int? id)
        {
            //var id = Request.Query["id"];

            if (id != null)
            {
                //update
                var product = _storeContext.Products.Find(id.Value);
                ViewBag.Product = product;
            }
            else
            {
                //Insert
            }
            return View();
        }

        [HttpPost]
        public IActionResult InsertProduct(string name, string desc, decimal price)
        {
            _storeContext.Products.Add(new Product { Name = name, Desc = desc, Price = price });

            if (name.Length <= 50)
            {
                _storeContext.SaveChanges();
                return Json(new { isSuccess = true });
            }
            else
            {
                return Json(new { isSuccess = false });
            }
        }

        [HttpPost]
        public IActionResult UpdateProduct(int productId, string name, string desc, decimal price)
        {
            _storeContext.Products.Update(new Product { Id = productId, Name = name, Desc = desc, Price = price });

            if (name.Length <= 50)
            {
                _storeContext.SaveChanges();
                return Json(new { isSuccess = true });
            }
            else
            {
                return Json(new { isSuccess = false });
            }
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {

            var product = _storeContext.Products.Find(id);
            if (product != null)
            {
                _storeContext.Products.Remove(product);
                _storeContext.SaveChanges();
                return Json(new { isSuccessDeleted = true });
            }
            else
            {
                return Json(new { isSuccessDeleted = false });
            }
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
    }
}
