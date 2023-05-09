using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public TransactionController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var transactionList = _db.Transactions.Where(x => x.TenantId == _userId).ToList();

            var transactionAllList = (from t in transactionList
                                     join p in _db.PaymentModes on t.PaymentModeId equals p.Id
                                     select new
                                     {
                                         Id = t.Id,
                                         StartDateString = t.StartDate.ToShortDateString(),
                                         EndDateString = t.EndDate.ToShortDateString(),
                                         TransactionDateString = t.TransactionDate.ToShortDateString(),
                                         Remarks = t.Remarks,
                                         PaymentMode = p.Name,
                                         Total = t.Total
                                     }).ToList();

            ViewData["Transaction"] = transactionAllList;

            decimal rentAmount = 0;
            var userRoom = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == _userId);
            if(userRoom != null && userRoom.HouseRoomId != null)
            {
                var houseRoom = await _db.HouseRooms.FirstOrDefaultAsync(x => x.Id == userRoom.HouseRoomId);
                var roomDetail = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == houseRoom.RoomId);
                rentAmount = roomDetail is null ? 0 : roomDetail.RentAmount;
            }

            ViewData["RentAmount"] = rentAmount;

            
            return View();
        }  

        

    }
}
