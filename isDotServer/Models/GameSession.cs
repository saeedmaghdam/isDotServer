using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class GameSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ViewId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public int HostId { get; set; }
        public int GuestId { get; set; }
        public Enums.Winner Winner { get; set; }
        public string WhosTurn { get; set; }

        //[ForeignKey("HostId")]
        public virtual User Host { get; set; }
        //[ForeignKey("GuestId")]
        public virtual User Guest { get; set; }
    }
}
