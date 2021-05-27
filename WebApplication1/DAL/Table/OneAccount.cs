namespace DAL.Table
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
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

        public int Sex { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string JotTitle { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string LicenseNo { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public byte DataState { get; set; }

        [StringLength(10)]
        public string IdNo { get; set; }

        [StringLength(20)]
        public string CardSerial { get; set; }

        public byte? LogIn { get; set; }
    }
}