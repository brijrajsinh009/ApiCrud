using System.ComponentModel.DataAnnotations;
using ApiCrud.Data.Models;
using Microsoft.AspNetCore.Http;

namespace ApiCrud.Data.CustomModels;

public class BookViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(75, ErrorMessage = "Maximum 75 characters are allowed for Name.")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(75, ErrorMessage = "Maximum 75 characters are allowed for Author Name.")]
    public string? Author { get; set; }

    [Required]
    [Range(1, 999, ErrorMessage = "Enter the Price beetween 1 to 999")]
    public decimal? Price { get; set; }

    // public IFormFile? ImageFile { get; set; }

    public string? Image { get; set; }
}
