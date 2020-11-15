using System;
using System.ComponentModel.DataAnnotations;

namespace BP_OnlineDOD_Server.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int ThumbsUpCount { get; set; }

        [Required]
        public DateTime TimeSent { get; set; }

        public Message()
        {
            this.TimeSent = DateTime.UtcNow;
        }
    }
}