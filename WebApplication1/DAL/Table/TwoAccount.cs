namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public partial class TwoAccount
    {
        public int Id { get; set; }

        [Column("Account")]
        [Required]
        [StringLength(50)]
        public string Account1 { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string IdNo { get; set; }

        public int Sex { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public int State { get; set; }

        public int AddOperater { get; set; }

        public DateTime AddDate { get; set; }

        public int? UpdateOperater { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Remark { get; set; }

        public DateTime? PWExpireDate { get; set; }
    }
}