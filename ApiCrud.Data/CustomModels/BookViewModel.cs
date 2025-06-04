using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.CustomModels;

public class BookViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string? Author { get; set; }

    [Required]
    [Range(1, 999, ErrorMessage = "Enter the Price beetween 1 to 999")]
    public decimal? Price { get; set; }
}
