using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{

    public class Wedding
    {

        [Key] 
        public int WeddingId { get; set; }
        public string Wedder1 { get; set; }
        public string Wedder2 { get; set; }
        public string Address { get; set; }
        public DateTime WeddingDate { get; set; }
        public User Creator {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Guest> Attendees { get; set; }


    }
}