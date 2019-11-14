using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class CustomRepository : Repository<User>, IRepository<User>
    {
        public CustomRepository(Context dbContext) : base(dbContext)
        {

        }
    }
}
