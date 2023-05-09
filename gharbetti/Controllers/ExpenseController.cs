using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;
        private readonly UserManager<IdentityUser> _userManager;


        public ExpenseController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _userManager = userManager;

        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            // Resolve the user via their email
            var user = await _userManager.FindByIdAsync(_userId);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            ViewData["CurrentRole"] = roles;



            return View();
        }  

        

    }
}
