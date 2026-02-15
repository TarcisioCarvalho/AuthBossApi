using System.ComponentModel.DataAnnotations;

namespace AuthBoss.Communication.Requests.User;
public class RequestRegisterUserJson
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public DateTime BirthDate { get; set; }
}
