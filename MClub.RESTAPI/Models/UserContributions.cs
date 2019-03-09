using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MClub.RESTAPI.Models
{
    public class UserContributions
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public string LatestContribution { get; set; }
        public string Image { get; set; }
    }
}