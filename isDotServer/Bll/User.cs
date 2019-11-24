using isDotServer.DataStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Bll
{
    public class User
    {
        private static ConcurrentList2<Models.User> cache = null;
        private static ConcurrentList2<Models.User> usersInQueue = null;

        IUnitOfWork _unitOfWork;
        IRepository<Models.User> repo;
        public User(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            if (cache == null)
                cache = new ConcurrentList2<Models.User>();
            if (usersInQueue == null)
                usersInQueue = new ConcurrentList2<Models.User>();
            repo = _unitOfWork.GetRepository<Models.User>(hasCustomRepository: true);
        }

        public Models.User Get(Models.User user, bool AddIfNotExists = false)
        {
            Models.User result = new Models.User();
            IEnumerable<Models.User> query = cache.Select(x => x);
            if (!string.IsNullOrEmpty(user.Username))
                query = cache.Where(x => x.Username == user.Username);
            if (!string.IsNullOrEmpty(user.UniqueId))
                query = cache.Where(x => x.UniqueId == user.UniqueId);
            if (query.Any())
                result = query.First();
            else
            {
                query = repo.GetAll();
                if (!string.IsNullOrEmpty(user.Username))
                    query = cache.Where(x => x.Username == user.Username);
                if (!string.IsNullOrEmpty(user.UniqueId))
                    query = cache.Where(x => x.UniqueId == user.UniqueId);
                if (query.Any())
                    result = query.First();
                else
                {
                    var newUser = new Models.User()
                    {
                        Username = user.Username,
                        UniqueId = user.UniqueId,
                        ConnectionId = user.ConnectionId,
                        ViewId = Guid.NewGuid()
                    };
                    repo.Insert(newUser);
                    _unitOfWork.SaveChanges();

                    result = newUser;
                }

                cache.Add(result);
            }

            return result;
        }

        public Models.User GetFirstWaitingUser()
        {
            if (usersInQueue.Any())
                return usersInQueue.First();
            else return null;
        }

        public void RemoveUserFromWaitingQueue(Models.User user)
        {
            var userInQueueQuery = usersInQueue.Where(x => x.Id == user.Id);
            if (userInQueueQuery.Any())
            {
                var userInQueue = userInQueueQuery.First();
                usersInQueue.Remove(userInQueue);
            }
        }

        public void AddUserToWaitingQueue(Models.User user)
        {
            usersInQueue.Add(user);
        }
    }
}
