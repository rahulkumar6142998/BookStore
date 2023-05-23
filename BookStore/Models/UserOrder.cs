using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class UserOrder
    {
        public List<BookModel> Products { get; set; }

        public string Title { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
