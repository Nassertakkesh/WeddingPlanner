using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    [NotMapped]
    public class LoginUser
    {
      [Required(ErrorMessage = "is required.")]
      [EmailAddress]
      [Display(Name = "Email")]
      public string LoginEmail { get; set; }

      [Required(ErrorMessage = "is required.")]
      [MinLength(3, ErrorMessage = "must be at least 8 characters")]
      [DataType(DataType.Password)] // auto fills input type attr
      [Display(Name = "Password")]
      public string LoginPassword { get; set; }
    }
}