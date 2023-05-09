using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ComplainController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public ComplainController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public IActionResult Index()
        {
            if (this.User.IsInRole("tenant"))
            {
                var complainList = (from c in _db.Complains
                                    join ap in _db.ApplicationUsers on c.TenantId equals ap.Id
                                    join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                    join r in _db.Rooms on hr.RoomId equals r.Id
                                    join h in _db.Houses on hr.HouseId equals h.Id
                                    where c.TenantId == _userId
                                    select new
                                    {
                                        c.Id,
                                        TenantName = ap.FirstName,
                                        TenantId = ap.LastName,
                                        c.Reason,
                                        c.Response,
                                        c.ComplainDate,
                                        c.Status,
                                        House = h.Name,
                                        Room = r.RoomNo
                                    }).ToList();

                ViewData["Complain"] = complainList;
            }
            else if (this.User.IsInRole("admin"))
            {
                var complainList = (from c in _db.Complains
                                    join ap in _db.ApplicationUsers on c.TenantId equals ap.Id
                                    join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                    join r in _db.Rooms on hr.RoomId equals r.Id
                                    join h in _db.Houses on hr.HouseId equals h.Id
                                    orderby c.ComplainDate descending
                                    select new
                                    {
                                        c.Id,
                                        TenantName = ap.FirstName,
                                        TenantId = ap.LastName,
                                        c.Reason,
                                        c.Response,
                                        c.ComplainDate,
                                        c.Status,
                                        House = h.Name,
                                        Room = r.RoomNo
                                    }).ToList();
                                        
                    //.OrderByDescending(x => x.ComplainDate).ToList();
                ViewData["Complain"] = complainList;
            }

            return View();
        }



    }
}
