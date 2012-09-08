using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkOnce.Models
{
    public class Link
    {
        static DateTime DefaultUsedDate = DateTime.Parse("1753-01-01");
        public Link()
        {
            DateUsed = DefaultUsedDate;
            DateCreated = DateTime.Now;
        }
        [Key]
        public int LinkId { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string ShortUrl { get; set; }
        public string OptionalEmailAddress { get; set; }
        public bool IsUsed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUsed { get; set; }

    }
}