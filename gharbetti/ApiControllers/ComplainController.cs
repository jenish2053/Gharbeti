using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public ComplainController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] ComplainViewModel model)
        {
            _db.Complains.Add(new Complain
            {
                Reason = model.Reason,
                Response = null,
                ComplainDate = DateTime.Now,
                Status = model.Status,
                TenantId = _userId
            });
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true , Message = "Data Added Sucessfully" });
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.Complains.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                return Ok(new { Data = editData, Status = true });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }


        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody] ComplainViewModel model)
        {

            var editData = _db.Complains.FirstOrDefault(x => x.Id == model.Id);
            if (editData != null)
            {
                editData.Reason = model.Reason;
                editData.Status = model.Status;
                editData.Response = model.Response;

                _db.Complains.Update(editData);
                _db.SaveChanges();
                return Ok(new { Data = model, Status = true });
            }
            else
            {
                return Ok(new { Data = model, Status = false, Message = "Error while saving!!!" });
            }

        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.Complains.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                if (!string.IsNullOrEmpty(editData.Response))
                {
                    return Ok(new { Status = false, Message = "Landlord response complain cannot be delete!!!" });
                }
                if (editData.Status != 0)
                {
                    return Ok(new { Status = false, Message = "Change Status cannot be deleted!!!" });
                }

                _db.Remove(editData);
                _db.SaveChanges();
                return Ok(new { Data = editData, Status = true, Message = "Deleted Successfully!!!" });
            }
            else
            {
                return Ok(new { Status = false, Message = "Error Occured!!!" });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allComplain = _db.Complains.ToList();
            return Ok(new { Data = allComplain, Status = true, Message = "Data Loaded " });
        }
    }
}
