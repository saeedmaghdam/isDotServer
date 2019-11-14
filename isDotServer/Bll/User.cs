using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Bll
{
    public class User
    {
        public void Add()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<Models.Context>();
            //optionsBuilder.UseSqlServer("Server=.;Database=isDotGame;User ID=sa;Password=Abcd@123456;TrustServerCertificate=True;Trusted_Connection=False;Connection Timeout=30;Integrated Security=False;Persist Security Info=False;Encrypt=True;MultipleActiveResultSets=True;", x => x.UseRowNumberForPaging());
            //optionsBuilder.UseSqlite("Data Source=blog.db");

            //using (var ctx = new Models.Context(optionsBuilder.Options))
            //using (var ctx = new Models.Context())
            //{
            //    ctx.Users.Add(new Models.User()
            //    {
            //        //Id = 1,
            //        ViewId = Guid.NewGuid(),
            //        UniqueId = Guid.NewGuid().ToString()
            //    });
            //    ctx.SaveChanges();
            //}
        }
    }
}
