using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ExpenseTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpenseTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public IActionResult Index()
        {
            var expenseTypeList = _db.ExpenseTypes.ToList();
            ViewData["ExpenseType"] = expenseTypeList;
            return View();
        }  

        

    }
}
