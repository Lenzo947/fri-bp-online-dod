using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP_OnlineDOD.Shared.Models
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


        public int? ParentMessageId { get; set; }

        public Message ParentMessage { get; set; }

        public ICollection<Message> ChildMessages { get; set; }

        public Message()
        {
            this.TimeSent = DateTime.UtcNow;
            this.ChildMessages = new List<Message>();
        }
    }
}