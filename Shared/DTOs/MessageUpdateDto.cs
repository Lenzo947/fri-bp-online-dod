using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Shared.DTOs
{
    public class MessageUpdateDto
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int ThumbsUpCount { get; set; }

        [Required]
        public bool Deleted { get; set; }

    }
}
