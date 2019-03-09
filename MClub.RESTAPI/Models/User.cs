using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MClub.RESTAPI.Models
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}