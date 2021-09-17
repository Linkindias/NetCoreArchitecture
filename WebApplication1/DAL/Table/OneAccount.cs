namespace DAL.Table
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OneAccount")]
    public partial class OneAccount
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
        public int Sex { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateId { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateId { get; set; }

        [Required]
        public int State { get; set; }

        [Required]
        [StringLength(10)]
        public string IdNo { get; set; }
    }
}