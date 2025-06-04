using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.CustomModels;

public class LoginDetails
{
  [Required(ErrorMessage = "Email is required")]
  [RegularExpression(@"^\S*$", ErrorMessage = "Email cannot contain spaces.")]
  public string UserEmail { get; set; }

  [Required(ErrorMessage = "Password is required")]
  public string Password { get; set; }
}

