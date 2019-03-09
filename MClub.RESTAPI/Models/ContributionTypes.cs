using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MClub.RESTAPI.Models
{
   // [Serializable]
    public class ContributionTypes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string SchemaUrl { get; set; }
    }
}