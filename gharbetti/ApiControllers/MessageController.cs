using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Gharbetti.ApiControllers.ExpenseController;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public MessageController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            if (httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MessageViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var addedMessage = await _db.Message.AddAsync(new Message
                {
                    Subject = model.Subject,
                    Body = model.Body,
                    PostedDate = DateTime.Now,
                    IsAll = model.IsAll
                });

                _db.SaveChanges();

                if (model.IsAll)
                {
                    var allUser = (from usr in _db.Users
                                   join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                   join role in _db.Roles on userRole.RoleId equals role.Id
                                   join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                   join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                   join room in _db.Rooms on hr.RoomId equals room.Id
                                   join h in _db.Houses on hr.HouseId equals h.Id
                                   where role.Name.ToLower() == "tenant"
                                   select new
                                   {
                                       UserId = usr.Id,
                                   }).ToList();

                    foreach (var item in allUser)
                    {
                        await _db.TenantMessages.AddAsync(new TenantMessage
                        {
                            MessageId = addedMessage.Entity.Id,
                            TenantId = item.UserId,
                            Status = 1
                        });
                    }

                }
                else
                {
                    //var allUser = (from usr in _db.Users
                    //               join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                    //               join role in _db.Roles on userRole.RoleId equals role.Id
                    //               join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                    //               join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                    //               join room in _db.Rooms on hr.RoomId equals room.Id
                    //               join h in _db.Houses on hr.HouseId equals h.Id
                    //               where role.Name.ToLower() == "tenant" && h.Id == model.HouseId
                    //               select new
                    //               {
                    //                   UserId = usr.Id,
                    //               }).ToList();

                    foreach (var item in model.Tenant)
                    {
                        await _db.TenantMessages.AddAsync(new TenantMessage
                        {
                            MessageId = addedMessage.Entity.Id,
                            TenantId = item.Id,
                            Status = 1
                        });
                    }


                }
                await _db.SaveChangesAsync();
                dbTran.Commit();
                return Ok(new { Data = model, Status = true, Message = "Message saved Sucessfully!!!" });
            }
            catch (Exception)
            {
                dbTran.Rollback();
                return Ok(new { Data = model, Status = false, Message = "Error while Saving!!!" });
            }

        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var editData = await _db.Message.FirstOrDefaultAsync(x => x.Id == id);

            if (editData == null)
            {
                return Ok(new { Status = false, Message = "Data Not Found!!!" });
            }

            var editViewData = new MessageViewModel
            {
                Id = editData.Id,
                IsAll = editData.IsAll,
                Body = editData.Body,
                PostedDate = editData.PostedDate,
                Subject = editData.Subject,
            };

            if (!editData.IsAll)
            {
                var allMessageTenant = _db.TenantMessages.Where(x => x.MessageId == editData.Id).Select(x => new MessageTenantViewModel
                {
                    Id = x.TenantId
                }).ToList();

                editViewData.Tenant = allMessageTenant;
            }
            else
            {
                editViewData.Tenant = new List<MessageTenantViewModel>();
            }

            return Ok(new { Data = editViewData, Status = true });

        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] MessageViewModel model)
        {
            using (var dbContext = _db.Database.BeginTransaction())
            {
                try
                {
                    var savedEditData = await _db.Message.FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (savedEditData != null)
                    {
                        savedEditData.Subject = model.Subject;
                        savedEditData.Body = model.Body;
                        savedEditData.IsAll = model.IsAll;

                        _db.Message.Update(savedEditData);


                        var allHouseList = await _db.TenantMessages.Where(x => x.MessageId == model.Id).ToListAsync();
                        if (allHouseList.Count > 0)
                        {
                            _db.TenantMessages.RemoveRange(allHouseList);
                        }


                        if (model.IsAll)
                        {
                            var allUser = (from usr in _db.Users
                                           join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                                           join role in _db.Roles on userRole.RoleId equals role.Id
                                           join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                                           join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                           join room in _db.Rooms on hr.RoomId equals room.Id
                                           join h in _db.Houses on hr.HouseId equals h.Id
                                           where role.Name.ToLower() == "tenant"
                                           select new
                                           {
                                               UserId = usr.Id,
                                           }).ToList();

                            foreach (var item in allUser)
                            {
                                await _db.TenantMessages.AddAsync(new TenantMessage
                                {
                                    MessageId = savedEditData.Id,
                                    TenantId = item.UserId,
                                    Status = 1
                                });
                            }

                        }
                        else
                        {
                            //var allUser = (from usr in _db.Users
                            //join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                            //join role in _db.Roles on userRole.RoleId equals role.Id
                            //join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                            //join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                            //join room in _db.Rooms on hr.RoomId equals room.Id
                            //join h in _db.Houses on hr.HouseId equals h.Id
                            //where role.Name.ToLower() == "tenant" && h.Id == model.HouseId
                            //select new
                            //{
                            //    UserId = usr.Id,
                            //}).ToList();

                            foreach (var item in model.Tenant)
                            {
                                await _db.TenantMessages.AddAsync(new TenantMessage
                                {
                                    MessageId = savedEditData.Id,
                                    TenantId = item.Id,
                                    Status = 1
                                });
                            }


                        }
                    }

                    _db.SaveChanges();
                    dbContext.Commit();
                    return Ok(new { Data = model, Status = true, Message = "Data Updated Successfully!!!" });
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    return Ok(new { Data = model, Status = false, Message = "Error Occured" });
                }
            }
        }


        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var editData = _db.Message.FirstOrDefault(x => x.Id == id);

            if (editData != null)
            {
                using (var dbContext = _db.Database.BeginTransaction())
                {
                    try
                    {
                        _db.Message.Remove(editData);

                        var tenantMessageData = _db.TenantMessages.Where(x => x.MessageId == editData.Id);
                        _db.TenantMessages.RemoveRange(tenantMessageData);


                        _db.SaveChanges();
                        dbContext.Commit();
                        return Ok(new { Data = editData, Status = true, Message = "Deleted Successfully!!!" });
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return Ok(new { Data = editData, Status = false, Message = "Error Occured!!!" });
                    }
                }
            }
            else
            {
                return Ok(new { Status = false, Message = "Data Not Found!!!" });
            }
        }


        [HttpGet]
        [Route("GetHouses")]
        public IActionResult GetHouses()
        {

            var houseList = _db.Houses.ToList();

            return Ok(new { Status = true, Message = "Data Load Sucessfully", Data = houseList });
        }


        [HttpGet]
        [Route("GetMessage")]
        public IActionResult GetMessage()
        {
            try
            {
                var currentUserId = _userId;
                var messageList = (from mess in _db.Message
                                   join tm in _db.TenantMessages on mess.Id equals tm.MessageId
                                   where tm.TenantId == currentUserId
                                   select new
                                   {
                                       mess.Subject,
                                       mess.Body,
                                       PostedDateString = mess.PostedDate.ToShortDateString(),
                                       mess.PostedDate
                                   }).OrderByDescending(x => x.PostedDate).ToList();

                return Ok(new { Status = true, Message = "Data Loaded Sucessfully", Data = messageList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error Occured" });
            }


        }
    }
}
