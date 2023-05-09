using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public IActionResult Index()
        {
            var roomList = _db.Rooms.ToList();
            ViewData["Room"] = roomList;
            return View();
        }  

        

    }
}
