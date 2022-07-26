using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class StatusDto
    {
       [Required] public string status { get; set; }
       [Required] public string color { get; set; }
       public bool IsActive { get; set; }
    }
}