using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Data;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ApproveController(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
                        RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [HttpGet]
        [Route("Register")]
        public async Task<IActionResult> Register(string id)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var tenantRole = await _roleManager.FindByNameAsync(StaticDetail.Role_Tenant);
                var pendingCurrentRole = await _roleManager.FindByNameAsync(StaticDetail.Role_PendingTenant);
                var user = await _userManager.FindByIdAsync(id);

                await _userManager.RemoveFromRoleAsync(user, pendingCurrentRole.Name);
                await _userManager.AddToRoleAsync(user, tenantRole.Name);

                StripeConfiguration.ApiKey = "sk_test_51MrMYfG57wLeiTAQci6zNIOYQS40rPL4IfQawYqzWZNfTdPDA3enyrfVLFrQ8LqNHrTiE0eH8lMYpVJaJRmmwNJU00uqBJ2Qv2";
                var applicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == user.Id);
                var options = new CustomerCreateOptions
                {
                    Name = applicationUser.FirstName + " " + applicationUser.LastName,
                    Email = user.Email,
                };

                var service = new CustomerService();
                Customer customer = service.Create(options);

                applicationUser.CustomerId = customer.Id;
                _db.ApplicationUsers.Update(applicationUser);
                _db.SaveChanges();
                dbTran.Commit();
                return Ok(new { Status = true, Message = "Role Changed Sucessfully and Customer Created" });
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                return Ok(new { Status = false, Message = "Error while Changing role" });
            }
        }


        [HttpGet]
        [Route("Resubmit")]
        public async Task<IActionResult> Resubmit(string userId, string remarks)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userApplication = await _db.ApplicationUsers.FirstAsync(x => x.Id == userId);

            if (userApplication != null)
            {
                userApplication.ApproveRemarks = remarks;

                _db.ApplicationUsers.Update(userApplication);
                _db.SaveChanges();
                return Ok(new { Status = true, Message = "Remarks Send Successfully!!!" });
            }

            return Ok(new { Status = false, Message = "Error while saving remarks" });
        }

      

    }
}
