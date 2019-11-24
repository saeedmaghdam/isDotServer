using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(Context dbContext) : base(dbContext)
        {

        }
    }
}
