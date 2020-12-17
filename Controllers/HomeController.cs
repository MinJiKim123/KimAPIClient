using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KimMinAPIClient.Models;
using System.Net.Http;
using KimMinAPIClient.Services;
using System.Globalization;

namespace KimMinAPIClient.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient client;
        private HttpClientService service;
        public HomeController()
        {
            client = new HttpClient();
            service = new HttpClientService(client);
        }

        public async Task<IActionResult> Index()
        {
            int cMonth = DateTime.Now.Month;
            int cYear = DateTime.Now.Year;
            string scMonth = DateTime.Now.ToString("MMMM");
            var exps = await GetExpensesByMonth(cMonth, cYear);
            double total_exp_p = 0;
            foreach (var i in exps)
                total_exp_p += i.Price;
            IndexViewModel ivm = new IndexViewModel
            {
                CurrentMonth = scMonth + " " + cYear,
                Expenses = exps,
                TotalExpense = Math.Round(total_exp_p, 2)
            };
            return View(ivm);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Price,Date")] Expense expense)
        {
            await service.CreateExpenseAsync(expense);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Expense temp = await service.GetExpenseAsync(id);

            return View(temp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Content, Price, Date")] ExpenseToUpdateDto expense2update)
        {
            var res = await service.UpdateExpenseAsync(id, expense2update);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            bool res = await service.DeleteExpenseAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ViewLog()
        {
            DateTime now = DateTime.Now;
            DateTime currentMF = new DateTime(now.Year, now.Month, 1);
            var exps = await GetExpensesBeforeMonth(currentMF);
            return View(exps);

        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        private async Task<IEnumerable<Expense>> GetExpensesByMonth(int month, int year)
        {
            List<Expense> finalExps = new List<Expense>();
            var allexpenses = await service.GetExpensesAsync();
            foreach (var ae in allexpenses)
            {
                DateTime aeDate = DateTime.ParseExact(ae.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (aeDate.Month == month && aeDate.Year == year)
                {
                    finalExps.Add(ae);
                }
            }
            return finalExps;
        }
        private async Task<IEnumerable<Expense>> GetExpensesBeforeMonth(DateTime date)
        {
            List<Expense> finalExps = new List<Expense>();
            var allexpenses = await service.GetExpensesAsync();
            foreach (var ae in allexpenses)
            {
                DateTime aeDate = DateTime.ParseExact(ae.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (DateTime.Compare(date, aeDate) > 0)
                    finalExps.Add(ae);
            }
            return finalExps;
        }
    }
}
