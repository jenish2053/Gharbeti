using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class PaymentModeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaymentModeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public IActionResult Index()
        {
            var paymentModeList = _db.PaymentModes.ToList();
            ViewData["PaymentMode"] = paymentModeList;
            return View();
        }  

        

    }
}
