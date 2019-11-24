using isDotServer.DataStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Bll
{
    public class GameSession
    {
        private static ConcurrentList2<Models.GameSession> cache;

        IUnitOfWork _unitOfWork;
        IRepository<Models.GameSession> repo;
        public GameSession(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            if (cache == null)
                cache = new ConcurrentList2<Models.GameSession>();
            repo = _unitOfWork.GetRepository<Models.GameSession>(hasCustomRepository: true);
        }

        public Models.GameSession Insert(Models.User host, Models.User guest)
        {
            var viewId = Guid.NewGuid();
            var result = new Models.GameSession()
            {
                StartTime = DateTime.UtcNow,
                ViewId = viewId,
                HostId = host.Id,
                GuestId = guest.Id,
                Winner = Enums.Winner.NOT_COMPLETED,
                WhosTurn = "host"
            };
            repo.Insert(result);
            _unitOfWork.SaveChanges();

            var query = repo.GetAll().Include("Host").Include("Guest").Where(x => x.ViewId == viewId);
            if (query.Any())
                result = query.First();

            cache.Add(result);

            return result;
        }

        public Models.GameSession Get(Guid viewId)
        {
            Models.GameSession result = null;
            IEnumerable<Models.GameSession> query = cache.Where(x => x.ViewId == viewId);
            if (query.Any())
                result = query.First();
            else
            {
                query = repo.GetAll().Include("Host").Include("Guest").Where(x => x.ViewId == viewId);
                if (query.Any())
                    result = query.First();
            }

            return result;
        }

        public void ChangeTurn(Guid viewId)
        {
            var gameSession = Get(viewId);
            if (gameSession != null)
            {
                if (gameSession.WhosTurn == "host")
                    gameSession.WhosTurn = "guest";
                else
                    gameSession.WhosTurn = "host";
            }
        }

        public void ChangeTurn(Models.GameSession gameSession)
        {
            if (gameSession.WhosTurn == "host")
                gameSession.WhosTurn = "guest";
            else
                gameSession.WhosTurn = "host";
        }
    }
}
