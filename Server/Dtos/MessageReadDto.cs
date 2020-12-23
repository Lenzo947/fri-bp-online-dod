using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Server.Dtos
{
    public class MessageReadDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int ThumbsUpCount { get; set; }

        public int? ParentMessageId { get; set; }

        public DateTime TimeSent { get; set; }

        public Message ParentMessage { get; set; }

        public ICollection<Message> ChildMessages { get; set; }
    }
}