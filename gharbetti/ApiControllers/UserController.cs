using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public UserController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        [HttpGet]
        [Route("gettenant")]
        public async Task<IActionResult> GetTenant()
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var tenantList = (from au in _db.ApplicationUsers
                                  join ur in _db.UserRoles on au.Id equals ur.UserId
                                  join r in _db.Roles on ur.RoleId equals r.Id
                                  where r.Name == StaticDetail.Role_Tenant
                                  select new
                                  {
                                      au.Id,
                                      Name = au.FirstName + " " + au.LastName,
                                  }).ToList();

                return Ok(new { Status = true, Message = "Sucessfully", Data = tenantList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error while Changing role" });
            }
        }


        [HttpGet]
        [Route("gettenantsamehouse")]
        public async Task<IActionResult> GetTenantSameHouse()
        {
            try
            {
                var sameHouseId = await (from ap in _db.ApplicationUsers
                                         join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                         join h in _db.Houses on hr.HouseId equals h.Id
                                         where ap.Id == _userId
                                         select hr).FirstOrDefaultAsync();


                var tenantList = (from au in _db.ApplicationUsers
                                  join ur in _db.UserRoles on au.Id equals ur.UserId
                                  join r in _db.Roles on ur.RoleId equals r.Id
                                  join hr in _db.HouseRooms on au.HouseRoomId equals hr.Id
                                  join h in _db.Houses on hr.HouseId equals h.Id
                                  where r.Name == StaticDetail.Role_Tenant && h.Id == sameHouseId.HouseId 
                                  select new
                                  {
                                      au.Id,
                                      Name = au.FirstName + " " + au.LastName,
                                  }).ToList();

                return Ok(new { Status = true, Message = "Sucessfully", Data = tenantList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error while Changing role" });
            }
        }

    }
}
