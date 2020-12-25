using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public class GameService : IGameService
    {
        public List<Lobby> Lobbies { get; set; }
        public List<Player> Players { get; set; }

        public GameService()
        {
            Lobbies = new List<Lobby>();
            Players = new List<Player>();
        }

        public void AddMessage(Message message, string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            lobby.Messages.Add(message);
        }

        public ICollection<Message> GetMessages(string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            return lobby.Messages;
        }

        public void AddSegmentPoint(Point point, string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));

            if (point.IsFirstPointInSegment)
            {
                lobby.LineSegments.Add(new LineSegment());
            }

            lobby.LineSegments[lobby.LineSegments.Count - 1].Points.Add(point);
        }

        public Lobby AddLobby(string lobbyName)
        {
            Lobby lobby = new Lobby();
            lobby.Id = Guid.NewGuid().ToString();
            lobby.Name = lobbyName;
            lobby.Players = new List<Player>();
            lobby.Messages = new List<Message>();
            lobby.LineSegments = new List<LineSegment>();
            this.Lobbies.Add(lobby);

            return lobby;
        }

        public bool RemoveLobby(string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));

            if (lobby == null)
            {
                return false;
            }

            return Lobbies.Remove(lobby);
        }

        public IEnumerable<Lobby> GetLobbies()
        {
            return this.Lobbies;
        }

        public Lobby GetLobby(string lobbyId)
        {
            return Lobbies.SingleOrDefault(l => l.Id.Equals(lobbyId));
        }

        public Player CreatePlayer(string playerName, string connectionId)
        {
            var player = new Player()
            {
                Id = Guid.NewGuid().ToString(),
                Name = playerName,
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

        public Lobby GetPlayerLobby(string playerConnectionId)
        {
            var lobby = Lobbies.Find(l => l.Players.Find(p => p.ConnectionId.Equals(playerConnectionId)) != null);

            return lobby;
        }

        public Player AddPlayerToLobby(string lobbyId, string playerConnectionId)
        {
            var player = Players.Find(p => p.ConnectionId.Equals(playerConnectionId));
            var lobby = this.Lobbies.Find(l => l.Id.Equals(lobbyId));

            if (player == null || lobby == null)
                return null;

            lobby.Players.Add(player);

            return player;
        }

        public Player RemovePlayerFromLobby(string lobbyId, string playerConnectionId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            var player = Players.Find(p => p.ConnectionId.Equals(playerConnectionId));

            if (lobby == null || player == null)
                return null;

            lobby.Players.RemoveAll(p => p.Id.Equals(player.Id));

            return player;
        }

        public Lobby StartGame(string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            lobby.InGame = true;
            lobby.LineSegments = new List<LineSegment>();

            return lobby;
        }

        public Lobby StopGame(string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            lobby.InGame = false;
            lobby.LineSegments = new List<LineSegment>();

            return lobby;
        }

        public string GetLobbyName(string lobbyId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));

            if (lobby == null)
            {
                return null;
            }

            return lobby.Name;
        }
    }
}
