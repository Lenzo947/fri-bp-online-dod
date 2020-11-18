using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Message
{
    public class Message
    {
        public virtual int Id { get; set; }

        [Required]
        [StringLength(100000, ErrorMessage = "Správa musí obsahovať aspoň 5 znakov!", MinimumLength = 5)]
        public virtual string Text { get; set; }

        public virtual int ThumbsUpCount { get; set; }

        public virtual DateTime TimeSent { get; set; }
    }
}
