using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Helpers
{
    public class Mapper
    {
        public static LobbyDto MapLobbyToLobbyDto(Lobby lobby)
        {
            var lobbyDto = new LobbyDto();
            lobbyDto.Id = lobby.Id;
            lobbyDto.Name = lobby.Name;
            lobbyDto.InGame = lobby.InGame;
            lobbyDto.Players = lobby.Players;

            return lobbyDto;
        }
    }
}
