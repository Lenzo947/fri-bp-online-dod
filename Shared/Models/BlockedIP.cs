using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BP_OnlineDOD.Shared.Models
{
    public class BlockedIP
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
