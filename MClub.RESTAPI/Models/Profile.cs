using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MClub.RESTAPI.Models
{
     public class Profile
    {
        public string DisplayName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string About { get; set; }
        public string Level { get; set; }
        public string mCoins { get; set; }
        public string Tier { get; set; }
        public bool DownloadedTheApp { get; set; }
        public bool WebLogin { get; set; }
        public string Image { get; set; }

    }
}
