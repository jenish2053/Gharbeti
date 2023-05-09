using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class CleanScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public CleanScheduleController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var currentDetail = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == _userId);
            var currentHouse = await _db.HouseRooms.FirstOrDefaultAsync(x => x.Id == currentDetail.HouseRoomId);

            var allCleanScheduleList = _db.CleanSchedules.Select(x=> new
            {
                x.Id,
               x.TenantId,
               x.CreatedBy,
                x.StartDate,
                x.EndDate,
            }).ToList();

            var cleanScheduleList = (from cs in allCleanScheduleList
                                     join us in _db.ApplicationUsers on cs.TenantId.ToLower() equals us.Id
                                     join uss in _db.ApplicationUsers on cs.CreatedBy.ToLower() equals uss.Id
                                     join hr in _db.HouseRooms on uss.HouseRoomId equals hr.Id
                                     where hr.HouseId == currentHouse.HouseId
                                     select new
                                     {
                                         Id = cs.Id,
                                         StartDate = cs.StartDate.ToShortDateString(),
                                         EndDate = cs.EndDate.ToShortDateString(),
                                         Tenant = us.FirstName + " " + us.LastName,
                                         CreatedBy = uss.FirstName + " " + uss.LastName,
                                     }).ToList();

            ViewData["CleanSchedule"] = cleanScheduleList;
            return View();
        }



    }
}
