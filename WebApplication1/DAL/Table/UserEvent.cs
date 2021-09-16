using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Table
{
    [Table("UserEventLog")]
    public partial class UserEventLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Machine { get; set; }

        [Required]
        [StringLength(20)]
        public string IP { get; set; }

        [Required]
        [StringLength(100)]
        public string PageCode { get; set; }

        [Required]
        [StringLength(30)]
        public string Method { get; set; }

        public int CreateId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}