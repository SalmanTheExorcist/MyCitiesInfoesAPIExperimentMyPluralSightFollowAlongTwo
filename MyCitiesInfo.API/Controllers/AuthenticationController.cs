using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyCitiesInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ??
                                throw new ArgumentNullException(nameof(configuration));
        }



        //--------------------------------------------
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
                        [FromBody] AuthenticationRequestBody authenticationRequestBody)
        {

            //--Step-1: Validate the Username and Password:
            var myUser = ValidateUserCredentials(
                            authenticationRequestBody.UserName,
                            authenticationRequestBody.Password);


            if (myUser == null)
            {
                return Unauthorized();
            }


            //--Step-2: Create a token
            var mySecurityKey = new SymmetricSecurityKey(
                   Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));

            var mySigningCredentials = new SigningCredentials(
                            mySecurityKey, SecurityAlgorithms.HmacSha256);

            //--Creating the claims
            var myClaimsForToken = new List<Claim>();
            //--sub: is standardized key for a unique-user-identifier
            myClaimsForToken.Add(new Claim("sub", myUser.UserId.ToString()));
            myClaimsForToken.Add(new Claim("given_name", myUser.FirstName));
            myClaimsForToken.Add(new Claim("family_name", myUser.LastName));
            myClaimsForToken.Add(new Claim("city", myUser.City));

            //--
            var myJwtSecurityToken = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],                    
                    myClaimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    mySigningCredentials);

            //---
            var myTokenToReturn = new JwtSecurityTokenHandler()
                                        .WriteToken(myJwtSecurityToken);

            return Ok(myTokenToReturn);



        }//--End-HTTP-POST

        private MyCityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            //--For demo purposes, we will assume the credentials are valid
            //---we are focusing on how to create a token for now.
            return new MyCityInfoUser(
                            1,
                            userName ?? "",
                            "MrSpooky",
                            "TheExorcist",
                            "Manama,BH");



        }//--End-ValidateUserCredentials-Method

        //--we won't be using this outside of our controller
        //--so we are scoping it here
        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }//--End-Class-AuthenticationRequestBody

        private class MyCityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public MyCityInfoUser(int userId,
                                  string userName,
                                  string firstName,
                                  string lastName,
                                  string city)
            {
                this.UserId = userId;
                this.UserName = userName;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.City = city;
            }
        }




    }//--End-Class
}//--End-Namespace
