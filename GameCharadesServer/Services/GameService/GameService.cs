using GameCharadesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Services
{
    public class GameService : IGameService
    {
        private ILobbiesService lobbiesService;

        public GameService(ILobbiesService lobbiesService)
        {
            this.lobbiesService = lobbiesService;
        }

        public void AddMessage(Message message, string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);
            lobby.Messages.Add(message);
        }

        public ICollection<Message> GetMessages(string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);
            return lobby.Messages;
        }

        public void AddSegmentPoint(Point point, string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);

            if (point.IsFirstPointInSegment)
            {
                lobby.LineSegments.Add(new LineSegment());
            }

            lobby.LineSegments[lobby.LineSegments.Count - 1].Points.Add(point);
        }

        public Lobby StartGame(string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);
            lobby.InGame = true;
            lobby.LineSegments = new List<LineSegment>();

            return lobby;
        }

        public Lobby StopGame(string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);
            lobby.InGame = false;
            lobby.LineSegments = new List<LineSegment>();

            return lobby;
        }
    }
}
