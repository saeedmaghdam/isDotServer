using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class GameSessionRepository : Repository<GameSession>, IRepository<GameSession>
    {
        public GameSessionRepository(Context dbContext) : base(dbContext)
        {

        }
    }
}
