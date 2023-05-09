using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class FloorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FloorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public IActionResult Index()
        {
            var floorList = _db.Floors.ToList();
            ViewData["Floor"] = floorList;
            return View();
        }  

        

    }
}
