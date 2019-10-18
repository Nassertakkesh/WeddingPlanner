using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{

    public class Guest
    {
////////////////////////////////////////////////////
        [Key] // denotes PK, not needed if named ModelNameId
        public int GuestId { get; set; }
        public int WeddingId { get; set; }
        public int UserId { get; set; }
        public User Attendees { get; set; }
        public Wedding Event { get; set; }


    }
}