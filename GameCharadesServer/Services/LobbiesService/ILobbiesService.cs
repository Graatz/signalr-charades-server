
using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public interface ILobbiesService
    {
        Lobby AddLobby(string lobbyName);
        bool RemoveLobby(string lobbyId);
        string GetLobbyName(string lobbyId);
        IEnumerable<Lobby> GetLobbies();
        Lobby GetLobby(string lobbyId);
        Lobby GetPlayerLobby(string playerConnectionId);
        Player AddPlayerToLobby(string lobbyId, string playerConnectionId);
        Player RemovePlayerFromLobby(string lobbyId, string playerConnectionId);
    }
}
