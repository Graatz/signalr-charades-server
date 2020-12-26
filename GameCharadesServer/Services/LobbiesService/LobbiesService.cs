using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public class LobbiesService : ILobbiesService
    {
        public List<Lobby> Lobbies { get; set; }

        private IPlayersService playersService;

        public LobbiesService(IPlayersService playersService)
        {
            this.playersService = playersService;

            Lobbies = new List<Lobby>();
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

        public Lobby GetPlayerLobby(string playerConnectionId)
        {
            var lobby = Lobbies.Find(l => l.Players.Find(p => p.ConnectionId.Equals(playerConnectionId)) != null);

            return lobby;
        }

        public Player AddPlayerToLobby(string lobbyId, string playerConnectionId)
        {
            var player = playersService.GetPlayerByConnectionId(playerConnectionId);
            var lobby = this.Lobbies.Find(l => l.Id.Equals(lobbyId));

            if (player == null || lobby == null)
                return null;

            lobby.Players.Add(player);

            return player;
        }

        public Player RemovePlayerFromLobby(string lobbyId, string playerConnectionId)
        {
            var lobby = Lobbies.Find(l => l.Id.Equals(lobbyId));
            var player = playersService.GetPlayerByConnectionId(playerConnectionId);

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
