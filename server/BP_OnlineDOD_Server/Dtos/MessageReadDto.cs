using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD_Server.Dtos
{
    public class MessageReadDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int ThumbsUpCount { get; set; }

        public DateTime TimeSent { get; set; }
    }
}