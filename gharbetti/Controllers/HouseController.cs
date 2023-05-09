using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class HouseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HouseController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public IActionResult Index()
        {
            var houseList = _db.Houses.ToList();
            ViewData["House"] = houseList;
            return View();
        }  

        

    }
}
