using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;
        private readonly UserManager<IdentityUser> _userManager;


        public MessageController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _userManager = userManager;

        }

        [Authorize(Roles = "admin")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var messageList = (from m in _db.Message
                         //      join h in _db.Houses on m.HouseId equals h.Id into houses
                           //    from hr in houses.DefaultIfEmpty()
                               select new
                               {
                                   m.Id,
                                   m.Subject,
                                   m.Body,
                                   PostedDate = m.PostedDate.ToShortDateString(),
                                   PostDate = m.PostedDate,
                             //      House = m.HouseId == 0 ? "All" : hr.Name
                               }).OrderByDescending(x => x.PostDate).ToList();

            ViewData["Message"] = messageList;
            return View();
        }  

        

    }
}
