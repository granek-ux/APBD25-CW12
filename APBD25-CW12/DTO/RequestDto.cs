using Microsoft.Build.Framework;

namespace APBD25_CW12.DTO;

public class RequestDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Telephone { get; set; }
    [Required]
    public string Pesel { get; set; }
    [Required]
    public int IdTrip { get; set; }
    [Required]
    public string TripName { get; set; } 
    public DateTime? PaymentDate { get; set; }
}