using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public class PlayersService : IPlayersService
    {
        public List<Player> Players { get; set; }

        public PlayersService()
        {
            Players = new List<Player>();
        }

        public Player GetPlayerByConnectionId(string playerConnectionId)
        {
            return Players.Find(p => p.ConnectionId.Equals(playerConnectionId));
        }

        public Player CreatePlayer(string playerName, string avatar, string connectionId)
        {
            var player = new Player()
            {
                Id = Guid.NewGuid().ToString(),
                Name = playerName,
                Avatar = avatar,
                ConnectionId = connectionId
            };

            Players.Add(player);

            return player;
        }

        public bool RemovePlayer(string connectionId)
        {
            var player = Players.Find(p => p.ConnectionId.Equals(connectionId));

            if (player == null)
            {
                return false;
            }

            Players.Remove(player);

            return true;
        }
    }
}
