using System.ComponentModel.DataAnnotations;

namespace WebSocketChat.Models
{
    public class LoginForm
    {
        [Required, MinLength(1)] public string Name { get; set; }
        [Required, MinLength(3), MaxLength(20)] public string Password { get; set; }
    }
}