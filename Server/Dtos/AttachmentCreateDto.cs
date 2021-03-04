using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Dtos
{
    public class AttachmentCreateDto
    {
        [Required]
        public string ContentType { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int MessageId { get; set; }
    }
}
