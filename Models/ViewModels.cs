using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KimMinAPIClient.Models
{
    public class IndexViewModel
    {
        public string CurrentMonth { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
        public double TotalExpense { get; set; }

    }
    public class ExpenseToUpdateDto
    {
        public string Content { get; set; }
        public string Date { get; set; }
        public double Price { get; set; }
    }
}
