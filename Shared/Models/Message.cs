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
        [StringLength(10000, ErrorMessage = "Správa presahuje povolený počet znakov! (10000)")]
        [MinLength(5, ErrorMessage = "Správa musí obsahovať aspoň 5 znakov!")]
        public string Text { get; set; }

        [Required]
        public int ThumbsUpCount { get; set; }

        [Required]
        public DateTime TimeSent { get; set; }

        [Required]
        public bool Deleted { get; set; }


        public int? ParentMessageId { get; set; }

        public Message ParentMessage { get; set; }

        public ICollection<Message> ChildMessages { get; set; }

        public Message()
        {
            this.TimeSent = DateTime.UtcNow;
            this.ChildMessages = new List<Message>();
        }
    }

    public class MessageComparer : IEqualityComparer<Message>
    {
        public bool Equals(Message m1, Message m2)
        {
            return 
                m1.Id == m2.Id &&
                m1.Text == m2.Text &&
                m1.ThumbsUpCount == m2.ThumbsUpCount &&
                m1.TimeSent == m2.TimeSent &&
                m1.Deleted == m2.Deleted;
        }

        public int GetHashCode(Message m)
        {
            return m.Id;
        }
    }
}