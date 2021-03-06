using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Dtos
{
    public class AttachmentReadDto
    {
        public int Id { get; set; }

        public string ContentType { get; set; }

        public string Content { get; set; }

        public int MessageId { get; set; }

        //public Message Message { get; set; }
    }
}
