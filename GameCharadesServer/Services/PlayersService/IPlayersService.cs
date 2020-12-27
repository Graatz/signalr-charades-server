
using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public interface IPlayersService
    {
        Player GetPlayerByConnectionId(string playerConnectionId);
        Player CreatePlayer(string playerName, string avatar, string connectionId);
        bool RemovePlayer(string connectionId);
    }
}
