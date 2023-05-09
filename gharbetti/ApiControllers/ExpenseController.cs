using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public ExpenseController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }


        [HttpPost]
        [Route("GetTenantRange")]
        public IActionResult GetTenantRange([FromBody] FilterViewModel filterData)
        {
            try
            {
                var transactionList = _db.Transactions.Where(x => x.TenantId == _userId).ToList();
                if (filterData.FilterType == 0)
                {
                    var sdate = DateTime.Parse(filterData.StartDateString);
                    var edate = DateTime.Parse(filterData.EndDateString);


                    var transactionAllList = (from t in transactionList
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              where t.TransactionDate.Date >= sdate && t.TransactionDate.Date <= edate
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  RentPaid = t.RentPaid,
                                                  Total = t.Total
                                              }).ToList();

                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });
                }
                else if (filterData.FilterType == 1 && filterData.Month != 0)
                {
                    var transactionAllList = (from t in transactionList
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              where t.TransactionDate.Month == filterData.Month && t.TransactionDate.Year == t.TransactionDate.Year
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  RentPaid = t.RentPaid,
                                                  Total = t.Total
                                              }).ToList();

                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });

                }
                else
                {
                    var transactionAllList = (from t in transactionList
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              where t.TransactionDate.Year == t.TransactionDate.Year
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  RentPaid = t.RentPaid,
                                                  Total = t.Total
                                              }).ToList();

                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error Occured" });
            }
        }

        [HttpGet]
        [Route("ShowTranDetail")]
        public IActionResult ShowTranDetail(int id)
        {
            var editData = _db.Transactions.FirstOrDefault(x => x.Id == id);

            var transactionDetailData = _db.TransactionDetails.Where(x => x.TransactionId == id).Select(x => new TransactionDetailViewModel
            {
                ExpenseId = x.ExpenseId,
                Amount = x.Amount,
                Remarks = x.Remarks,
                TransactionId = id,
            }).ToList();


            if (editData != null)
            {
                return Ok(new { Data = transactionDetailData, Status = true });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }

        }



        [HttpPost]
        [Route("GetLandlordRange")]
        public IActionResult GetLandlordRange([FromBody] FilterViewModel filterData)
        {
            try
            {

                if (filterData.FilterType == 0)
                {
                    var sdate = DateTime.Parse(filterData.StartDateString);
                    var edate = DateTime.Parse(filterData.EndDateString);


                    var transactionAllList = (from t in _db.Transactions
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              join ap in _db.ApplicationUsers on t.TenantId equals ap.Id
                                              join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                              join r in _db.Rooms on hr.RoomId equals r.Id
                                              join h in _db.Houses on hr.HouseId equals h.Id
                                              where t.TransactionDate.Date >= sdate && t.TransactionDate.Date <= edate
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  Total = t.Total,
                                                  RentPaid = t.RentPaid,
                                                  House = h.Name,
                                                  Room = r.RoomNo
                                              }).ToList();

                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });
                }
                else if (filterData.FilterType == 1 && filterData.Month != 0)
                {
                    var transactionAllList = (from t in _db.Transactions
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              join ap in _db.ApplicationUsers on t.TenantId equals ap.Id
                                              join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                              join r in _db.Rooms on hr.RoomId equals r.Id
                                              join h in _db.Houses on hr.HouseId equals h.Id
                                              where t.TransactionDate.Month == filterData.Month && t.TransactionDate.Year == t.TransactionDate.Year
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  RentPaid = t.RentPaid,
                                                  Total = t.Total,
                                                  House = h.Name,
                                                  Room = r.RoomNo
                                              }).ToList();



                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });

                }
                else
                {
                    var transactionAllList = (from t in _db.Transactions
                                              join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                              join ap in _db.ApplicationUsers on t.TenantId equals ap.Id
                                              join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                                              join r in _db.Rooms on hr.RoomId equals r.Id
                                              join h in _db.Houses on hr.HouseId equals h.Id
                                              where t.TransactionDate.Year == t.TransactionDate.Year
                                              select new
                                              {
                                                  Id = t.Id,
                                                  StartDateString = t.StartDate.ToShortDateString(),
                                                  EndDateString = t.EndDate.ToShortDateString(),
                                                  TransactionDateString = t.TransactionDate.ToShortDateString(),
                                                  Remarks = t.Remarks,
                                                  PaymentMode = p.Name,
                                                  RentPaid = t.RentPaid,
                                                  Total = t.Total,
                                                  House = h.Name,
                                                  Room = r.RoomNo
                                              }).ToList();

                    return Ok(new { Data = transactionAllList, Status = true, Message = "Data Loaded " });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error Occured" });
            }
        }



        [HttpPost]
        [Route("GetPaidStatus")]
        public async Task<IActionResult> GetPaidStatus([FromBody] FilterViewModel filterData)
        {
            try
            {
                var allUser = (from usr in _db.Users
                               join userRole in _db.UserRoles on usr.Id equals userRole.UserId
                               join role in _db.Roles on userRole.RoleId equals role.Id 
                               join ap in _db.ApplicationUsers on usr.Id equals ap.Id
                               join hr in _db.HouseRooms on ap.HouseRoomId equals hr.Id
                               join room in _db.Rooms on hr.RoomId equals room.Id
                               join h in _db.Houses on hr.HouseId equals h.Id
                               where role.Name.ToLower() == "tenant"
                               select new RentPaidViewModel
                               {
                                   Tenant = ap.FirstName + " " + ap.LastName,
                                   UserId = usr.Id,
                                   House = h.Name,
                                   Room = room.RoomNo,
                                   Status = "UnPaid",
                                   RentAmount = room.RentAmount.ToString("0.00"),
                                   RentPaid = "0"

                               }).ToList();
  

                var allSameMonthTransaction = _db.Transactions.Where(x => allUser.Select(z => z.UserId).Contains(x.TenantId) && x.TransactionDate.Month == filterData.Month);

                foreach (var item in allUser)
                {
                    var matchTransaction = allSameMonthTransaction.FirstOrDefault(x => x.TenantId == item.UserId);
                    if (matchTransaction != null)
                    {
                        item.Status = "Paid";
                        item.RentAmount = matchTransaction.RentAmount.ToString("0.00");
                        item.RentPaid = matchTransaction.RentPaid.ToString("0.00");
                    }
                }

                return Ok(new { Data = allUser, Status = true, Message = "Data Loaded " });


            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error Occured" });
            }
        }


        public class RentPaidViewModel
        {
            public string UserId { get; set; }
            public string Tenant { get; set; }
            public string House { get; set; }
            public string Room { get; set; }
            public string Status { get; set; }
            public string RentAmount { get; set;}
            public string RentPaid { get; set; }

        }

    }
}
