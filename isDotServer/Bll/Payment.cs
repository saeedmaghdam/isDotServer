using isDotServer.DataStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Bll
{
    public class Payment
    {
        IUnitOfWork _unitOfWork;
        IRepository<Models.Payment> repo;
        public Payment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repo = _unitOfWork.GetRepository<Models.Payment>(hasCustomRepository: true);
        }

        public void Insert(Models.Payment payment)
        {
            repo.Insert(payment);
            _unitOfWork.SaveChanges();
        }
    }
}
