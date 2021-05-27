namespace DAL.TableModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WEBAPI_EVENT_LOGS")]
    public partial class ExceptionLog
    {
        [Key]
        public int SerialNo { get; set; }

        public DateTime UPD_DTM { get; set; }

        [Required]
        [StringLength(50)]
        public string UPD_OPER { get; set; }

        [Required]
        [StringLength(50)]
        public string OPR_IP { get; set; }

        [Required]
        public string ROUTE { get; set; }

        [Required]
        [StringLength(30)]
        public string METHOD { get; set; }

        [Required]
        public string PARAMETER { get; set; }

        [Required]
        public string EXCEPTION { get; set; }

        [Required]
        [StringLength(50)]
        public string OS { get; set; }
    }
}