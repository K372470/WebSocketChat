using System.ComponentModel.DataAnnotations;

namespace WebSocketChat.Models
{
    public class RegisterForm : LoginForm
    {
        [Compare(nameof(Password), ErrorMessage = "Passwords Dismatch"), Required] public string Password2 { get; set; }
    }
}
