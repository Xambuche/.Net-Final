using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [Required]

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }


        public int ParentID { get; set; }

        public int Orders { get; set; }
        [Required]

        public DateTime? Created_At { get; set; }

        public int? Updated_By { get; set; }

        public DateTime? Updated_At { get; set; }

        public int? Status { get; set; }
    }
}