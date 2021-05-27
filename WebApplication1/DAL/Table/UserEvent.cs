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
        public string NameMachine { get; set; }

        [Required]
        [StringLength(20)]
        public string IpAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string PageCode { get; set; }

        [Required]
        [StringLength(10)]
        public string Method { get; set; }

        [StringLength(100)]
        public string UserDepartment { get; set; }

        public int UserRoleId { get; set; }
        public int UserAccountId { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public string Logs { get; set; }

        public int State { get; set; }

        public int AddOperater { get; set; }

        public DateTime AddDate { get; set; }
    }
}