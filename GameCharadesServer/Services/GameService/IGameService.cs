
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
        Lobby StartGame(string lobbyId);
        Lobby StopGame(string lobbyId);
    }
}
