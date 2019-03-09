using MClub.RESTAPI.Models;
using MClub.RESTAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MClub.RESTAPI.Controllers
{
    public class AuthenticationController : ApiController
    {
        private IUserService _userService;

        [HttpPost]
        [ActionName("Authenticate")]
        public async Task<IHttpActionResult> Authenticate(string username, string password)
        {
            try
            {
                _userService = new UserService();
                var user = await _userService.Authenticate(username, password);

                return Ok(new AuthenticationResponse
                {
                    IsAuthenticated = true,
                    User = user
                });
            }
            catch (Exception ex)
            {
                return  Ok(new AuthenticationResponse
                {
                    IsAuthenticated = false,
                    Message = ex.Message
                });
            }
        }
    }
}
