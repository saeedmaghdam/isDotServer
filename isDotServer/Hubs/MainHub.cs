using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Hubs
{
    public class User
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    public class MainHub : Hub
    {
        IUnitOfWork _unitOfWork;
        Bll.User userBll;
        Bll.GameSession gameSessionBll;
        private static string hostId;

        private static readonly ConcurrentDictionary<string, User> Users
            = new ConcurrentDictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);

        public MainHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            userBll = new Bll.User(unitOfWork);
            gameSessionBll = new Bll.GameSession(unitOfWork);
        }

        // ************************************************************** //

        //public IEnumerable<string> GetConnectedUsers()
        //{

        //    return Users.Where(x => {

        //        lock (x.Value.ConnectionIds)
        //        {

        //            return !x.Value.ConnectionIds.Contains(Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase);
        //        }

        //    }).Select(x => x.Key);
        //}

        //private User GetUser(string username)
        //{

        //    User user;
        //    Users.TryGetValue(username, out user);

        //    return user;
        //}

        //public override Task OnConnectedAsync()
        //{
        //    string userName = Context.User.Identity.Name;
        //    string connectionId = Context.ConnectionId;

        //    var user = Users.GetOrAdd(userName, _ => new User
        //    {
        //        Name = userName,
        //        ConnectionIds = new HashSet<string>()
        //    });

        //    lock (user.ConnectionIds)
        //    {

        //        user.ConnectionIds.Add(connectionId);

        //        // // broadcast this to all clients other than the caller
        //        // Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);

        //        // Or you might want to only broadcast this info if this 
        //        // is the first connection of the user
        //        if (user.ConnectionIds.Count == 1)
        //        {

        //            Clients.Others.SendAsync("userConnected", userName);
        //        }
        //    }

        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{

        //    string userName = Context.User.Identity.Name;
        //    string connectionId = Context.ConnectionId;

        //    User user;
        //    Users.TryGetValue(userName, out user);

        //    if (user != null)
        //    {

        //        lock (user.ConnectionIds)
        //        {

        //            user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));

        //            if (!user.ConnectionIds.Any())
        //            {

        //                User removedUser;
        //                Users.TryRemove(userName, out removedUser);

        //                // You might want to only broadcast this info if this 
        //                // is the last connection of the user and the user actual is 
        //                // now disconnected from all connections.
        //                Clients.Others.SendAsync("userDisconnected", userName);
        //            }
        //        }
        //    }

        //    return base.OnDisconnectedAsync(exception);
        //}

        //public void Send(string message)
        //{

        //    string sender = Context.User.Identity.Name;

        //    // So, broadcast the sender, too.
        //    Clients.All.SendAsync("received", new { sender = sender, message = message, isPrivate = false });
        //}

        //public void Send(string message, string to)
        //{

        //    User receiver;
        //    if (Users.TryGetValue(to, out receiver))
        //    {

        //        User sender = GetUser(Context.User.Identity.Name);

        //        IEnumerable<string> allReceivers;
        //        lock (receiver.ConnectionIds)
        //        {
        //            lock (sender.ConnectionIds)
        //            {

        //                allReceivers = receiver.ConnectionIds.Concat(sender.ConnectionIds);
        //            }
        //        }

        //        foreach (var cid in allReceivers)
        //        {
        //            Clients.Client(cid).SendAsync("received", new { sender = sender.Name, message = message, isPrivate = true });
        //        }
        //    }
        //}

        // ************************************************************** //

        public async Task Test2(string param)
        {
            var v = param;

            var repo = _unitOfWork.GetRepository<Models.User>(hasCustomRepository: true);
            repo.Insert(new Models.User()
            {
                ViewId = Guid.NewGuid(),
                UniqueId = Guid.NewGuid().ToString(),
                Username = "Test"
            });
            _unitOfWork.SaveChanges();
        }

        public async Task Test(Boolean val)
        {
            await Clients.Groups(new List<string>()
                {
                    hostId
                }).SendAsync("ItsMyTurn");
        }

        public async Task Ack(string Name)
        {
            //await Clients.Others.SendAsync("AckResponse", "Hello dear client, I'm " + Name);
            await Clients.Others.SendAsync("AckResponse");
        }

        public async Task SayImHere()
        {
            //await Clients.Others.SendAsync("AckResponse", "Hello dear client, I'm " + Name);
            await Clients.Others.SendAsync("ImHere");
        }

        private async Task<Models.User> GetUser(string userUniqueId, string username)
        {
            // get current users information
            var user = userBll.Get(new Models.User()
            {
                Username = username,
                UniqueId = userUniqueId
            });
            await Groups.AddToGroupAsync(Context.ConnectionId, user.Id.ToString());

            return user;
        }

        public async Task RiseMyHand(string userUniqueId, string username)
        {
            var user = GetUser(userUniqueId, username).Result;

            // Check if there's anyone to play together
            // If yes, create a game session and send it's id to both players
            var host = userBll.GetFirstWaitingUser();
            if (userBll.GetFirstWaitingUser() != null && !user.UniqueId.Equals(host.UniqueId))
            {
                // Create a new game session
                var gameSession = gameSessionBll.Insert(host, user);

                // Remove the host user from queue
                userBll.RemoveUserFromWaitingQueue(host);

                // Inform both users to start the game and send relevent information including the campatitor's name, avatar, game session viewId
                await Clients.Groups(new List<string>()
                {
                    user.Id.ToString(),
                    host.Id.ToString()
                }).SendAsync("StartTheGame", gameSession.ViewId.ToString());

                //await Clients.Groups(new List<string>()
                //{
                //    host.Id.ToString()
                //}).SendAsync("ItsMyTurn");

                hostId = host.Id.ToString();
            }
            else
            {
                userBll.AddUserToWaitingQueue(user);
            }
        }

        public async Task WhoAmI(string userUniqueId, string username)
        {
            var user = GetUser(userUniqueId, username).Result;

            if (user != null)
            {
                await Clients.Groups(new List<string>()
                {
                    user.Id.ToString()
                }).SendAsync("WhoAmI", user.ViewId);
            }
        }

        public async Task WhosTurn(string userUniqueId, string username, string gameSessionViewId)
        {
            var user = GetUser(userUniqueId, username).Result;

            if (user != null)
            {
                Guid viewId = Guid.NewGuid();
                if (Guid.TryParse(gameSessionViewId, out viewId))
                {
                    var gameSession = gameSessionBll.Get(viewId);
                    if (gameSession != null)
                    {
                        Guid userViewId = Guid.Empty;
                        if (gameSession.WhosTurn == "host")
                            userViewId = gameSession.Host.ViewId;
                        else if (gameSession.WhosTurn == "guest")
                            userViewId = gameSession.Guest.ViewId;
                        await Clients.Groups(new List<string>()
                        {
                            user.Id.ToString()
                        }).SendAsync("WhosTurn", userViewId);
                    }
                }
            }
        }

        public async Task IPlayedMyTurn(string userUniqueId, string username, String gameSessionViewId, int selectedLineIndex)
        {
            var user = GetUser(userUniqueId, username).Result;

            if (user != null)
            {
                Guid viewId = Guid.Empty;
                if (Guid.TryParse(gameSessionViewId, out viewId))
                {
                    var gameSession = gameSessionBll.Get(viewId);
                    gameSessionBll.ChangeTurn(gameSession);

                    int userId = -1;
                    if (gameSession.WhosTurn == "host")
                        userId = gameSession.Host.Id;
                    else if (gameSession.WhosTurn == "guest")
                        userId = gameSession.Guest.Id;
                    await Clients.Groups(new List<string>()
                    {
                        userId.ToString()
                    }).SendAsync("ItsMyTurn", selectedLineIndex);
                }
            }
        }

        public async Task SelectALine(string userUniqueId, string username, string gameSessionViewId, int selectedLineIndex)
        {
            var user = GetUser(userUniqueId, username).Result;

            if (user != null)
            {
                Guid viewId = Guid.Empty;
                if (Guid.TryParse(gameSessionViewId, out viewId))
                {
                    var gameSession = gameSessionBll.Get(viewId);

                    int userId = -1;
                    if (gameSession.WhosTurn == "host")
                        userId = gameSession.Guest.Id;
                    else if (gameSession.WhosTurn == "guest")
                        userId = gameSession.Host.Id;
                    await Clients.Groups(new List<string>()
                    {
                        userId.ToString()
                    }).SendAsync("SelectALine", selectedLineIndex);
                }
            }
        }

        public async Task TerminateGame(string gameSessionViewId)
        {
            Guid viewId = Guid.Empty;
            if (Guid.TryParse(gameSessionViewId, out viewId))
            {
                var gameSession = gameSessionBll.Get(viewId);

                await Clients.Groups(new List<string>()
                    {
                        gameSession.GuestId.ToString(),
                        gameSession.HostId.ToString()
                    }).SendAsync("TerminateGame");
            }
        }
    }
}
