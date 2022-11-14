using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlogAPI.Controllers {

    /// <summary>
    /// Controller for Authorizing into the Blog API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        /// <summary>
        /// UserManager for the IdentityUsers.
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Configuration that holds info from appsettings.json
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Initializes the AuthController with a User Manager and the applications configuration.
        /// </summary>
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration) {
            _userManager = userManager;
            _configuration = configuration;
        }


        /// <summary>
        /// Performs an attempt at logging in based on client specified data.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post(Login model) {

            //Create a new response.
            Response response = new Response();

            //Perform a lookup for a user based on username.
            var user = await _userManager.FindByNameAsync(model.Username);

            //Verify if the user exists and the supplied password matches.
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) {

                //Create a new list of Auth Claims for the JWT.
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //Create the signing key for the JWT Token
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

                //Create the token.
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                //Create the return object.
                response.Data = new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };
            } else {
                response.StatusCode = 401;
                response.Message = "Invalid credentials were supplied";
            }

            //Respond to the client.
            return response;
        }
    }
}
