﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace day4.Models
{
    public partial class photo
    {
        [Key]
        public int id { get; set; }
        [StringLength(350)]
        public string title { get; set; }
        [StringLength(500)]
        public string bref { get; set; }
        [Column(TypeName = "money")]
        public decimal? price { get; set; }
        [StringLength(500)]
        public string img { get; set; }
        public int? author_id { get; set; }
        public int? cat_id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? date { get; set; }

        [ForeignKey("author_id")]
        [InverseProperty("photos")]
        public virtual user author { get; set; }
        [ForeignKey("cat_id")]
        [InverseProperty("photos")]
        public virtual catalog cat { get; set; }
    }
}