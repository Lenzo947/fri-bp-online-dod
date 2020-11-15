using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Message
{
    public class Message
    {
        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        public virtual int ThumbsUpCount { get; set; }

        public virtual DateTime TimeSent { get; set; }
    }
}
