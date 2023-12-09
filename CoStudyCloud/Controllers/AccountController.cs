using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoStudyCloud.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string? returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties()
                {
                    RedirectUri = Url.Action(nameof(ExternalLoginCallback), new { returnUrl })
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl)
        {
            //Check authentication response as mentioned on startup file as options.DefaultSignInScheme = "External"
            var authenticateResult = await HttpContext.AuthenticateAsync("External");

            if (!authenticateResult.Succeeded)
            {
                return BadRequest();
            }

            //check if principal value exists or not 
            if (authenticateResult.Principal != null)
            {
                //Check if the redirection has been done via google or any other links
                if (authenticateResult.Principal.Identities.ToList()[0].AuthenticationType?.ToLower() == "google")
                {
                    //get google account id for any operation to be carried out on the basis of the id
                    var googleAccountId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    //claim value initialization as mentioned on the startup file with options.DefaultScheme = "Application"
                    var claimsIdentity = new ClaimsIdentity("Application");

                    claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)!);// Given Name Of The User
                    claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Surname)!);// Surname Of The User
                    claimsIdentity.AddClaim(authenticateResult!.Principal.FindFirst(ClaimTypes.Email)!); // Email Address of The User

                    //TODO: Create user in database if it doesn't already exist

                    await HttpContext.SignInAsync("Application", new ClaimsPrincipal(claimsIdentity));

                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            //Check if any cookie value is present
            if (HttpContext.Request.Cookies.Any())
            {
                //Check for the cookie value with the name mentioned for authentication and delete each cookie
                var siteCookies =
                    HttpContext.Request.Cookies.Where(
                        c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));

                foreach (var cookie in siteCookies)
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }

            //sign out with any cookie present 
            await HttpContext.SignOutAsync("External");

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
