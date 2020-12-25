
using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public interface IGameService
    {
        void AddMessage(Message message, string lobbyId);

        public ICollection<Message> GetMessages(string lobbyId);

        public void AddSegmentPoint(Point point, string lobbyId);

        Lobby AddLobby(string lobbyName);

        bool RemoveLobby(string lobbyId);

        string GetLobbyName(string lobbyId);

        IEnumerable<Lobby> GetLobbies();
        Lobby GetLobby(string lobbyId);

        Player CreatePlayer(string playerName, string connectionId);

        bool RemovePlayer(string connectionId);

        Lobby GetPlayerLobby(string playerConnectionId);

        Player AddPlayerToLobby(string lobbyId, string playerConnectionId);

        Player RemovePlayerFromLobby(string lobbyId, string playerConnectionId);
        Lobby StartGame(string lobbyId);
        Lobby StopGame(string lobbyId);
    }
}
