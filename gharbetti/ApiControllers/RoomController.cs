using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RoomController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RoomViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var addedRoom = await _db.Rooms.AddAsync(new Room
                {
                    RoomNo = model.RoomNo,
                    FloorId = model.FloorId,
                    Remarks = model.Remarks,
                    RentAmount = model.RentAmount,
                    SquareFootage = model.SquareFootage,
                });
                _db.SaveChanges();
                foreach (var item in model.RoomDetails)
                {
                    await _db.RoomDetails.AddAsync(new RoomDetail
                    {
                        RoomId = addedRoom.Entity.Id,
                        RoomTypeId = item.RoomTypeId,
                        SquareFootage = item.SquareFootage
                    });
                }

                _db.SaveChanges();
                dbTran.Commit();
                return Ok(new { Data = model, Status = true, Message = "Room saved Sucessfully!!!" });
            }
            catch (Exception)
            {
                dbTran.Rollback();
                return Ok(new { Data = model, Status = false, Message = "Error while Saving!!!" });
            }
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int id)
        {
            var editData = _db.Rooms.FirstOrDefault(x => x.Id == id);

            var roomDetailData = _db.RoomDetails.Where(x => x.RoomId == id).Select(x => new RoomDetailViewModel
            {
                SquareFootage = x.SquareFootage,
                Id = id,
                RoomTypeId = x.RoomTypeId,
                RoomId = x.RoomId,
            }).ToList();

            var roomEditData = new RoomViewModel
            {
                Id = editData.Id,
                FloorId = editData.FloorId,
                Remarks = editData.Remarks,
                RentAmount = editData.RentAmount,
                RoomNo = editData.RoomNo,
                SquareFootage = editData.SquareFootage,
                RoomDetails = roomDetailData,
            };

            if (editData != null)
            {
                return Ok(new { Data = roomEditData, Status = true });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] RoomViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var savedEditData = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (savedEditData != null)
                {
                    savedEditData.Remarks = model.Remarks;
                    savedEditData.SquareFootage = model.SquareFootage;
                    savedEditData.RoomNo = model.RoomNo;
                    savedEditData.FloorId = model.FloorId;

                    _db.Rooms.Update(savedEditData);
                    _db.SaveChanges();

                    var deleteData = _db.RoomDetails.Where(x => x.Id == model.Id).ToList();
                    if (deleteData.Count > 0)
                    {
                        _db.RoomDetails.RemoveRange(deleteData);
                    }
                    _db.SaveChanges();

                    foreach (var item in model.RoomDetails)
                    {
                        await _db.RoomDetails.AddAsync(new RoomDetail
                        {
                            RoomId = savedEditData.Id,
                            RoomTypeId = item.RoomTypeId,
                            SquareFootage = item.SquareFootage
                        });
                    }
                }

                _db.SaveChanges();
                dbTran.Commit();
                return Ok(new { Data = model, Status = true, Message = "Data Updated Successfully!!!" });
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                return Ok(new { Data = model, Status = false, Message = "Error Occured" });
            }
        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.RoomTypes.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                _db.Remove(editData);
                _db.SaveChanges();
                return Ok(new { Data = editData, Status = true, Message = "Deleted Successfully!!!" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allRoom = _db.Rooms.ToList();
            return Ok(new { Data = allRoom, Status = true, Message = "Data Loaded " });
        }
    }
}
