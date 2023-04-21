using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.API
{
    public class BookApiResponseModel
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public ApiBookModel[] Items { get; set; }
    }
}
