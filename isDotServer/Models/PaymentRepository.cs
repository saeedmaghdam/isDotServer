using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class PaymentRepository : Repository<Payment>, IRepository<Payment>
    {
        public PaymentRepository(Context dbContext) : base(dbContext)
        {

        }
    }
}
