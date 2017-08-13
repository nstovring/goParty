using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using goPartyc3799bc758f544faa52787e94a24730dService.CustomAuth.Models;
using goPartyc3799bc758f544faa52787e94a24730dService.Models;
using Microsoft.Azure.Mobile.Server.Login;

namespace goPartyc3799bc758f544faa52787e94a24730dService.Controllers
{
    [Route(".auth/login/custom")]
    public class CustomAuthController : ApiController
    {
        private goPartyc3799bc758f544faa52787e94a24730dContext db;
        private string signingKey, audience, issuer;

        public CustomAuthController()
        {
            db = new goPartyc3799bc758f544faa52787e94a24730dContext();
            signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            var website = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            audience = $"https://{website}/";
            issuer = $"https://{website}/";
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] User body)
        {
            if (body == null || body.Username == null || body.Password == null || body.Username.Length == 0 || body.Password.Length == 0)
            {
                return BadRequest(); ;
            }

            if (!IsValidUser(body))
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, body.Username)
            };

            JwtSecurityToken token = AppServiceLoginHandler.CreateToken(
                claims, signingKey, audience, issuer, TimeSpan.FromDays(30));
            return Ok(new LoginResult()
            {
                AuthenticationToken = token.RawData,
                User = new LoginResultUser { UserId = body.Username }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

#pragma warning disable CSE0003 // Use expression-bodied members
        private bool IsValidUser(User user)
        {
            return db.Users.Count(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)) > 0;
        }
#pragma warning restore CSE0003 // Use expression-bodied members
    }
}