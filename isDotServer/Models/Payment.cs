using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ViewId { get; set; }
        public string RefId { get; set; }
        public string Amount { get; set; }
        public string Coins { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
