using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BP_OnlineDOD.Shared.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int MessageId { get; set; }

        public Message Message { get; set; }
    }
}
