using System.ComponentModel.DataAnnotations;

namespace practice_ef.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
}