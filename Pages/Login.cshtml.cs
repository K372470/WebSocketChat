using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebSocketChat.Models;
using WebSocketChat.Services;

namespace WebSocketChat.Pages
{
    [AllowAnonymous]
    public class LoginPageModel : PageModel
    {
        [BindProperty, Required]
        public LoginForm Form { get; set; }

        private readonly IDataBase db;

        public LoginPageModel(IDataBase db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnPost(LoginForm Form)
        {
            if (db.UserExists(Form.Name, Form.Password))
            {
                var claims = new List<Claim>() { new("usr", Form.Name) };
                var cp = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "usr", "role"));
                await HttpContext.SignInAsync(cp);
                return RedirectToPagePermanent("Chat");
            }
            else
            {
                ModelState.AddModelError(nameof(Form.Name), "User Not Found");
                return Page();
            }
        }
    }
}
