using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KimMinAPIClient.Models
{
    public class Expense
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public double Price { get; set; }
    }
}
