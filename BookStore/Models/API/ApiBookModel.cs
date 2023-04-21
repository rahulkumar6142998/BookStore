using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.API
{
    public class ApiBookModel
    {
        public string Id { get; set; }
        public VolumeInfoModel VolumeInfo { get; set; }
    }
}
