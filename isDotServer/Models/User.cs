using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ViewId { get; set; }
        public string UniqueId { get; set; }
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public int WinsCount { get; set; }
        public int FailsCount { get; set; }
        public int UnfinishedCount { get; set; }
        public int Rate { get; set; }

        public virtual ICollection<GameSession> GamesAsHost { get; set; }
        public virtual ICollection<GameSession> GamesAsGuest { get; set; }
    }
}
