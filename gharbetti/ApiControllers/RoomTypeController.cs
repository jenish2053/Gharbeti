using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RoomTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] RoomType model)
        {
            _db.RoomTypes.Add(model);
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true });
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.RoomTypes.FirstOrDefault(x => x.Id == id);

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
        public IActionResult Edit([FromBody] RoomType model)
        {
            _db.RoomTypes.Update(model);
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true });
        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.RoomTypes.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                _db.RoomTypes.Remove(editData);
                _db.SaveChanges();
                return Ok(new { Data = editData, Status = true, Message="Deleted Successfully!!!" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allFloor = _db.RoomTypes.ToList();
            return Ok(new { Data = allFloor, Status = true, Message = "Data Loaded " });
        }
    }
}
