using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebAPIIdentity.Areas.Identity.Data;
using WebAPIIdentity.Data;
using WebAPIIdentity.Models;

/*
 Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
 Install-Package Microsoft.EntityFrameworkCore.Tools

 Add-Migration CreateIdentitySchema
 Update-Database
 */
namespace WebAPIIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private WebAPIIdentityContext _dbContext;
        private readonly UserManager<WebAPIIdentityUser> _userManager;
        private readonly SignInManager<WebAPIIdentityUser> _signInManager;

        public ValuesController(WebAPIIdentityContext dbContext,
               UserManager<WebAPIIdentityUser> userManager,
               SignInManager<WebAPIIdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet("GetTopScores")]
        public ActionResult GetTopScores(int count)
        {
            List<Highscore> highscores = new List<Highscore>();
            if (count > 0)
            {
                highscores = _dbContext.Highscores.OrderByDescending(x => x.Score).Take(count).ToList();
            }
            else
            {
                highscores = _dbContext.Highscores.OrderByDescending(x => x.Score).ToList();
            }

            //List<Highscore> highscores = new List<Highscore>() { new Highscore("Yatzy", 310), new Highscore("I ❤️ Flutter", 301), new Highscore("Jesse", 304), new Highscore("和平 福", 300),
            //    new Highscore("4ever", 184), new Highscore("Yatzy", 200), new Highscore("Gizmo", 263), new Highscore("Jesse", 153), new Highscore("Lexicon", 305), new Highscore("Hello", 287) };
            return Ok(highscores);
        }

        [HttpPost("UpdateAndReturnHighscore")]
        public async Task<ActionResult<Highscore>> UpdateHighscore(Highscore highscore, int count)
        {
            _dbContext.Highscores.Add(highscore);
            await _dbContext.SaveChangesAsync();
            List<Highscore> highscores = new List<Highscore>();
            if (count > 0)
            {
                highscores = _dbContext.Highscores.OrderByDescending(x => x.Score).Take(count).ToList();
            }
            else
            {
                highscores = _dbContext.Highscores.OrderByDescending(x => x.Score).ToList();
            }
            return Ok(highscores);
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Ok("Error confirming your email.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            string statusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Ok(statusMessage);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] MyLoginModelType myLoginModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == myLoginModel.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, myLoginModel.Password, false);

                if (signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("MY_BIG_SECRET_KEY_KJHKIJHIKHOIH@£($)345435");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, myLoginModel.Email)
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return BadRequest("Password incorrect");
                }
            }
            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        public async Task<ActionResult> Signup([FromBody] MyLoginModelType myLoginModel)
        {
            WebAPIIdentityUser webAPIIdentityUser = new WebAPIIdentityUser()
            {
                Email = myLoginModel.Email,
                UserName = myLoginModel.Email,
            };

            var result = await _userManager.CreateAsync(webAPIIdentityUser, myLoginModel.Password);
            if (result.Succeeded)
            {
                //var userId = await _userManager.GetUserIdAsync(webAPIIdentityUser);
                //string returnUrl = Url.Content("~/");
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(webAPIIdentityUser);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = "https://localhost:44357/api/Values/ConfirmEmail?userId=" + userId + "&code=" + code;

                //SendEmail(myLoginModel.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").Wait();

                return Ok(new { Result = "Register Success" });
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    stringBuilder.Append(error.Description);
                }
                return BadRequest(new { Result = $"Register Fail: {stringBuilder.ToString()}" });
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string email)
        {

            if (email == null)
            {
                return Ok("Email cannot be null");
            }
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Ok("User Deleted");
                }
                else
                {
                    return BadRequest("Cannot delete user");
                }
            }
            return NotFound("User does not exist");
        }


        static async Task SendEmail(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jesse.burstrom@gmail.com", "Example User");
            //var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, "Example User");
            var plainTextContent = message; //"and easy to do anywhere, even with C#";
            var htmlContent = message;// "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
