using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Company.SampleApi.Entities;

namespace Company.SampleApi.Api.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IQueryable<User> _users;

        public LoginModel(IQueryable<User> users)
        {
            _users = users;
        }

        [BindProperty]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "Adresse e-mail invalide.")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; } = false;

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _users.Where(_ => _.Login == Email && _.Password == Password).FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Upn, user.Login)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(returnUrl ?? "/");
            }

            return Page();
        }
    }
}
