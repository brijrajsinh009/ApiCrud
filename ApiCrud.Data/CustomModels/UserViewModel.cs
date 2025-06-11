using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ApiCrud.Data.CustomModels;

public class UserViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^\S*$", ErrorMessage = "Email cannot contain spaces.")]
    public string UserEmail { get; set; } = null!;

    [Required]
    [MaxLength(75, ErrorMessage = "Maximum 75 characters are allowed for Name.")]
    public string Name { get; set; } = null!;

    [Required]
    [RegularExpression("^(\\+91[\\-\\s]?)?[0]?(91)?[789]\\d{9}$", ErrorMessage = "Enter valid phone number.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords not match with Confirm Password.")]
    public string ConfirmPassword { get; set; } = null!;

    public IFormFile? Image { get; set; }
}