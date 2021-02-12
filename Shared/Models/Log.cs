using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BP_OnlineDOD.Shared.Models
{
    public class Log
    {
        public int id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Timestamp { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Level { get; set; }

        [Column(TypeName = "text")]
        public string Template { get; set; }

        [Column(TypeName = "text")]
        public string Message { get; set; }

        [Column(TypeName = "text")]
        public string Exception { get; set; }

        [Column(TypeName = "text")]
        public string Properties { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime _ts { get; set; }
    }
}
