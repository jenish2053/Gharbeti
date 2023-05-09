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
    public class CleanScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public CleanScheduleController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] CleanScheduleViewModel model)
        {
            var startDate = DateTime.Parse(model.StartDateString);
            var endDate = DateTime.Parse(model.EndDateString);
            _db.CleanSchedules.Add(new CleanSchedule
            {
                CreatedBy =_userId,
                StartDate = startDate,
                EndDate = endDate,
                Remarks = model.Remarks,
                TenantId = model.TenantId
            });
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true, Message = "Data Saved Sucessfully!!!" });
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.CleanSchedules.FirstOrDefault(x => x.Id == id);
            
                            

            if (editData != null)
            {
                var editViewData = new CleanScheduleViewModel
                {
                    Id = editData.Id,
                    StartDate = editData.StartDate,
                    EndDate = editData.EndDate,
                    Remarks = editData.Remarks,
                    TenantId = editData.TenantId,
                    StartDateString = new DateOnly(editData.StartDate.Year, editData.StartDate.Month, editData.StartDate.Day).ToString(),
                    EndDateString = new DateOnly(editData.EndDate.Year, editData.EndDate.Month, editData.EndDate.Day).ToString()
                };
                return Ok(new { Data = editViewData, Status = true });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }


        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody] CleanScheduleViewModel model)
        {

            var editData = _db.CleanSchedules.FirstOrDefault(x => x.Id == model.Id);

            var startDate = DateTime.Parse(model.StartDateString);
            var endDate = DateTime.Parse(model.EndDateString);
            if (editData != null)
            {
                editData.StartDate = startDate;
                editData.EndDate = endDate;
                editData.Remarks = model.Remarks;
                editData.TenantId = model.TenantId;
                
                _db.CleanSchedules.Update(editData);
                _db.SaveChanges();
                return Ok(new { Data = model, Status = true, Message = "Data Updated Sucessfully!!!" });
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
            var editData = _db.CleanSchedules.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
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
            var allComplain = _db.CleanSchedules.ToList();
            return Ok(new { Data = allComplain, Status = true, Message = "Data Loaded " });
        }
    }
}
