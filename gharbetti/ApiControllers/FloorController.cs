using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FloorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] Floor model)
        {
            _db.Floors.Add(model);
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true });
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.Floors.FirstOrDefault(x => x.Id == id);

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
        public IActionResult Edit([FromBody] Floor model)
        {
            _db.Floors.Update(model);
            _db.SaveChanges();

            return Ok(new { Data = model, Status = true });
        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.Floors.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                _db.Remove(editData);
                _db.SaveChanges();
                return Ok(new { Data = editData, Status = true, Message="Delted Successfully!!!" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var allFloor = _db.Floors.ToList();
            return Ok(new { Data = allFloor, Status = true, Message = "Data Loaded " });
        }
    }
}
