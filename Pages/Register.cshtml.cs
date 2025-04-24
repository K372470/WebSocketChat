using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebSocketChat.Models;
using WebSocketChat.Services;

namespace WebSocketChat.Pages
{
    [AllowAnonymous]
    public class RegisterPageModel : PageModel
    {
        [BindProperty, Required]
        public RegisterForm Form { get; set; }

        private readonly IDataBase db;

        public RegisterPageModel(IDataBase db)
        {
            this.db = db;
        }

        public IActionResult OnPost(RegisterForm Form)
        {
            if (!ModelState.IsValid)
                return Page();
            if (db.UserExists(Form.Name))
                ModelState.AddModelError(nameof(Form.Name), "Already have user with the same nickname");
            else
            {
                db.RegisterUser(Form.Name, Form.Password);
                return RedirectToPagePermanentPreserveMethod("Login", "OnPost", new { Form = (LoginForm)Form });
            }
            return Page();
        }
    }
}
