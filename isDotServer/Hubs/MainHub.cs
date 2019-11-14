using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Hubs
{
    public class MainHub : Hub
    {
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

        public async Task LetMeJoinOnlineGames(string userUniqueId)
        {
            Console.WriteLine(userUniqueId);

            // Check if there's anyone to play together
            // If yes, create a game session and send it's id to both players

            // Save the user id into the database and queie
        }
    }
}
