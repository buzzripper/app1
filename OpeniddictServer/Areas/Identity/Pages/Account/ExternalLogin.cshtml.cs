#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using OpeniddictServer.Data;
using System.Security.Claims;

namespace OpeniddictServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account or auto-provision
                var email = info.Principal.FindFirstValue(ClaimTypes.Email) 
                    ?? info.Principal.FindFirstValue("email")
                    ?? info.Principal.FindFirstValue("preferred_username");

                if (string.IsNullOrEmpty(email))
                {
                    ErrorMessage = "Email claim not received from external provider.";
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                // Check if user exists by email
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    // Auto-provision: Create new user from external login
                    var name = info.Principal.FindFirstValue(ClaimTypes.Name)
                        ?? info.Principal.FindFirstValue("name")
                        ?? email;

                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true // Since they logged in via external provider, we trust the email
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        ErrorMessage = "Error creating user account.";
                        _logger.LogError("Error creating user for {Email}: {Errors}", email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }

                    _logger.LogInformation("User {Email} created account using {LoginProvider} provider.", email, info.LoginProvider);
                }

                // Add external login to user
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    ErrorMessage = "Error adding external login to user.";
                    _logger.LogError("Error adding external login for {Email}: {Errors}", email, string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                _logger.LogInformation("User {Email} logged in with {LoginProvider} provider.", email, info.LoginProvider);

                return LocalRedirect(returnUrl);
            }
        }
    }
}
