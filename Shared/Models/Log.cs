using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BP_OnlineDOD.Shared.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        [Column(TypeName = "Xml")]
        public string Properties { get; set; }
        public string LogEvent { get; set; }
    }
}
