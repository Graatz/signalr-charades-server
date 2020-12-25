using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class LobbyPlayer
    {
        public LobbyDto Lobby { get; set; }
        public Player Player { get; set; }

        public LobbyPlayer(LobbyDto lobby, Player player)
        {
            Lobby = lobby;
            Player = player;
        }
    }
}
