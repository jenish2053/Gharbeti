using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;


        public TransactionController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TransactionViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var startDate = DateTime.Parse(model.StartDateString);
                var endDate = DateTime.Parse(model.EndDateString);
                var tranDate = DateTime.Parse(model.TransactionDateString);

                var roomId = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == _userId);

                if (roomId == null)
                {
                    return Ok(new { Status = false, Message = "Error while getting room" });
                }

                var houseRoom = await _db.HouseRooms.FirstOrDefaultAsync(x => x.Id == roomId.HouseRoomId);
                if (houseRoom == null)
                {
                    return Ok(new { Status = false, Message = "Error while getting house room" });
                }

                var roomDetail = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == houseRoom.RoomId);

                var totalAmount = model.RentPaid + model.TransactionDetails.Sum(x => x.Amount);
                StripeConfiguration.ApiKey = "sk_test_51MrMYfG57wLeiTAQci6zNIOYQS40rPL4IfQawYqzWZNfTdPDA3enyrfVLFrQ8LqNHrTiE0eH8lMYpVJaJRmmwNJU00uqBJ2Qv2";
                var paymentMethodId = await _db.PaymentModes.FirstOrDefaultAsync(x => x.Id == model.PaymentModeId);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = Convert.ToInt64(totalAmount * 100),
                    Currency = "gbp",
                    PaymentMethod = paymentMethodId.StripePayment,
                    Customer = roomId.CustomerId,
                };
                var service = new PaymentIntentService();
                var payment = service.Create(options);


                var confirmptions = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = paymentMethodId.StripePayment,
                };
                var serviceToConfirm = new PaymentIntentService();
                service.Confirm(
                  payment.Id,
                  confirmptions);

                var presentTenantId = _userId.ToString();
                var addedTransaction = await _db.Transactions.AddAsync(new Transaction
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TransactionDate = tranDate,
                    RentPaid = model.RentPaid,
                    RoomId = roomDetail.Id,
                    TenantId = presentTenantId,
                    Total = totalAmount,
                    Remarks = model.Remarks,
                    RentAmount = model.RentAmount,
                    PaymentModeId = model.PaymentModeId,
                    StripePaymentId = payment.Id
                });
                _db.SaveChanges();

                foreach (var item in model.TransactionDetails)
                {
                    await _db.TransactionDetails.AddAsync(new TransactionDetail
                    {
                        TransactionId = addedTransaction.Entity.Id,
                        Amount = item.Amount,
                        ExpenseId = item.ExpenseId,
                        Remarks = item.Remarks,
                    });
                }

                _db.SaveChanges();
                dbTran.Commit();

                return Ok(new { Data = model, Status = true, Message = "Transaction saved Sucessfully!!!" });
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
            var editData = _db.Transactions.FirstOrDefault(x => x.Id == id);

            var transactionDetailData = _db.TransactionDetails.Where(x => x.TransactionId == id).Select(x => new TransactionDetailViewModel
            {
                ExpenseId = x.ExpenseId,
                Amount = x.Amount,
                Remarks = x.Remarks,
                TransactionId = id,
            }).ToList();

            var transactionEditData = new TransactionViewModel
            {
                Id = editData.Id,
                TransactionDetails = transactionDetailData,
                StartDateString = new DateOnly(editData.StartDate.Year, editData.StartDate.Month, editData.StartDate.Day).ToString(),
                EndDateString = new DateOnly(editData.EndDate.Year, editData.EndDate.Month, editData.EndDate.Day).ToString(),
                TransactionDateString = new DateOnly(editData.TransactionDate.Year, editData.TransactionDate.Month, editData.TransactionDate.Day).ToString(),
                Remarks = editData.Remarks,
                RentAmount = editData.RentAmount,
                RentPaid = editData.RentPaid,
                TransactionDate = editData.TransactionDate,
                StartDate = editData.StartDate,
                EndDate = editData.EndDate,
                RoomId = editData.RoomId,
                PaymentModeId = editData.PaymentModeId,
            };

            if (editData != null)
            {
                return Ok(new { Data = transactionEditData, Status = true });
            }
            else
            {
                return Ok(new { Status = true, Message = "Error Occured!!!" });
            }
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] TransactionViewModel model)
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var savedEditData = await _db.Transactions.FirstOrDefaultAsync(x => x.Id == model.Id);
                var paymentMethodId = await _db.PaymentModes.FirstOrDefaultAsync(x => x.Id == model.PaymentModeId);

                var startDate = DateTime.Parse(model.StartDateString);
                var endDate = DateTime.Parse(model.EndDateString);
                var tranDate = DateTime.Parse(model.TransactionDateString);

                if (savedEditData != null)
                {

                    if (savedEditData.PaymentModeId == model.PaymentModeId)
                    {
                        var options = new PaymentIntentUpdateOptions
                        {
                            Amount = Convert.ToInt64((model.RentPaid + model.TransactionDetails.Sum(x => x.Amount)) * 100)
                        };

                        var service = new PaymentIntentService();
                        service.Update(
                          savedEditData.StripePaymentId,
                          options);
                    }
                    else if(!String.IsNullOrEmpty(paymentMethodId.StripePayment) && savedEditData.PaymentModeId != model.PaymentModeId)
                    {
                        var options = new PaymentIntentUpdateOptions
                        {
                            Amount = Convert.ToInt64((model.RentPaid + model.TransactionDetails.Sum(x => x.Amount)) * 100),
                            PaymentMethod = paymentMethodId.StripePayment,
                        };

                        var service = new PaymentIntentService();
                        service.Update(
                          savedEditData.StripePaymentId,
                          options);

                        var confirmoptions = new PaymentIntentConfirmOptions
                        {
                            PaymentMethod = paymentMethodId.StripePayment,
                        };
                        var service2 = new PaymentIntentService();

                        service2.Confirm(
                          savedEditData.StripePaymentId,
                          confirmoptions);
                    }

                    savedEditData.Remarks = model.Remarks;
                    savedEditData.RentPaid = model.RentPaid;
                    savedEditData.RentAmount = model.RentAmount;
                    savedEditData.Total = model.RentPaid + model.TransactionDetails.Sum(x => x.Amount);
                    savedEditData.StartDate = startDate;
                    savedEditData.EndDate = endDate;
                    savedEditData.TransactionDate = tranDate;
                    savedEditData.PaymentModeId = model.PaymentModeId;
                    _db.Transactions.Update(savedEditData);

                    var deleteData = _db.TransactionDetails.Where(x => x.TransactionId == model.Id).ToList();
                    if (deleteData.Count > 0)
                    {
                        _db.TransactionDetails.RemoveRange(deleteData);

                    }

                    foreach (var item in model.TransactionDetails)
                    {
                        await _db.TransactionDetails.AddAsync(new TransactionDetail
                        {
                            TransactionId = savedEditData.Id,
                            Amount = item.Amount,
                            ExpenseId = item.ExpenseId,
                            Remarks = item.Remarks,
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
        public async Task<IActionResult> Delete(int id)
        {
            var editData = _db.Transactions.FirstOrDefault(x => x.Id == id);
            var dbTran = _db.Database.BeginTransaction();
            if (editData != null)
            {

                _db.Transactions.Remove(editData);

                var allTransactionDetail = await _db.TransactionDetails.Where(x => x.TransactionId == id).ToListAsync();

                _db.TransactionDetails.RemoveRange(allTransactionDetail);

                _db.SaveChanges();
                dbTran.Commit();
                return Ok(new { Data = editData, Status = true, Message = "Deleted Successfully!!!" });
            }
            else
            {
                dbTran.Rollback();
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
