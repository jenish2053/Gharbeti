using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Stripe;
using Gharbetti.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class StripePaymentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public StripePaymentController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {

            StripeConfiguration.ApiKey = "sk_test_51MrMYfG57wLeiTAQci6zNIOYQS40rPL4IfQawYqzWZNfTdPDA3enyrfVLFrQ8LqNHrTiE0eH8lMYpVJaJRmmwNJU00uqBJ2Qv2";
            var options = new PaymentIntentListOptions
            {
                Limit = 50,
            };
            var service = new PaymentIntentService();
            StripeList<PaymentIntent> paymentIntents = service.List(
              options);

            var viewList = new List<StripePaymentList>();

            foreach (var item in paymentIntents.Data)
            {
                if(item.Status != "succeeded" || item.Description == "test")
                {
                    continue;
                }

                
                var tenantData = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.CustomerId ==  item.CustomerId && item.CustomerId != null);
                var transactionData = await _db.Transactions.FirstOrDefaultAsync(x => x.StripePaymentId == item.Id);

                viewList.Add(new StripePaymentList
                {
                    Tenant = tenantData == null ? "Not Found" : tenantData.FirstName + " " + tenantData.LastName,
                    Amount = (item.Amount / 100).ToString("0.00"),
                    StartDate = transactionData == null ? "Not Found" : transactionData.StartDate.ToShortDateString(),
                    EndDate = transactionData == null ? "Not Found" : transactionData.EndDate.ToShortDateString(),
                    TransactionDate = transactionData == null ? "Not Found" : transactionData.TransactionDate.ToShortDateString(),
                });
            }

            ViewData["PaymentList"] = viewList;
            return View();
        }



    }
}
