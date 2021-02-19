using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Dtos
{
    public class MessageCreateDto
    {
        [Required]
        public string Author { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int ThumbsUpCount { get; set; }

        [Required]
        public bool Deleted { get; set; }

        public int? ParentMessageId { get; set; }

        public MessageCreateDto()
        {
            this.Deleted = false;
            this.ThumbsUpCount = 0;
        }

    }
}
