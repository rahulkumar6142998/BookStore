using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.API
{
    public class VolumeInfoModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string[] Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string PrintType { get; set; }
        public string[] Categories { get; set; }
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public ImageLinksModel ImageLinks { get; set; }
    }
}
