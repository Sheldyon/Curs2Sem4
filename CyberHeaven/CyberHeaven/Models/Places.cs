using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    [Table("Places")]
    public class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public PlaceCategory Category { get; set; }
    }
}
