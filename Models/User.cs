using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{

    public class User
    {
        [Key] // denotes PK, not needed if named ModelNameId
        public int UserId { get; set; }



        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "is required.")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "is required.")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "is required.")]
        [MinLength(3, ErrorMessage = "must be at least 3 characters")]
        [DataType(DataType.Password)] // auto fills input type attr
        public string Password { get; set; }


        [NotMapped] // don't add to DB
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "doesn't match password")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Guest> Events { get; set; }
        public List<Wedding> CreatedWedding {get;set;}




        public string FullName()
        {
            return FirstName + " " + LastName;
        }


    }
}