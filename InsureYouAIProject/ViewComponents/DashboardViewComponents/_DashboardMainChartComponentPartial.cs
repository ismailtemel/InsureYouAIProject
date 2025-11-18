using InsureYouAIProject.Context;
using InsureYouAIProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAIProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardMainChartComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardMainChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //revenue
            var revenuedata = await _context.Revenues.GroupBy(r => r.ProcessDate.Month).Select(g => new
            {
                Month = g.Key,
                TotalAmount = g.Sum(r => r.Amount)
            }).OrderBy(x => x.Month).ToListAsync();

            //expense
            var expensedata = await _context.Expenses.GroupBy(e => e.ProcessDate.Month).Select(g => new
            {
                Month = g.Key,
                TotalAmount = g.Sum(e => e.Amount)
            }).OrderBy(x => x.Month).ToListAsync();

            //allmonths
            var allMonths = revenuedata.Select(x => x.Month).Union(expensedata.Select(y => y.Month)).OrderBy(x => x).ToList();


            var model = new RevenueExpenseChartViewModel()
            {
                Months = allMonths.Select(x => new System.Globalization.CultureInfo("tr-TR").DateTimeFormat.GetAbbreviatedMonthName(x)).ToList(),
                RevenueTotals = allMonths.Select(m => revenuedata.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0).ToList(),
                ExpenseTotals = allMonths.Select(m => expensedata.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0).ToList()
            };

            ViewBag.sumAmount = await _context.Revenues.SumAsync(r => r.Amount);
            ViewBag.expensesSumAmount = await _context.Expenses.SumAsync(r => r.Amount);
            return View(model);
        }
    }
}