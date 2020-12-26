using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameCharadesServer.Helpers;
using GameCharadesServer.Models;
using GameCharadesServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameCharadesServer.Hubs
{
    public class GameHub : Hub
    {
        private IGameService gameService;
        private IPlayersService playersService;
        private ILobbiesService lobbiesService;

        public GameHub(IGameService gameService, IPlayersService playersService, ILobbiesService lobbiesService)
        {
            this.gameService = gameService;
            this.playersService = playersService;
            this.lobbiesService = lobbiesService;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var lobby = lobbiesService.GetPlayerLobby(Context.ConnectionId);

            // Remove player from lobby if he joined any
            if (lobby != null)
            {
                var player = lobbiesService.RemovePlayerFromLobby(lobby.Id, Context.ConnectionId);

                if (lobby.Players.Count == 0)
                {
                    lobbiesService.RemoveLobby(lobby.Id);
                    var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
                    Clients.All.SendAsync("LobbyRemoved", lobbyDto);
                }
                else
                {
                    Clients.Group(lobby.Name).SendAsync("PlayerLeftLobby", player);
                }

            }

            // Remove player
            playersService.RemovePlayer(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        #region Players
        public async Task CreatePlayer(string playerName)
        {
            var player = playersService.CreatePlayer(playerName, Context.ConnectionId);
            await Clients.Caller.SendAsync("PlayerCreated", player);
        }
        #endregion

        #region Lobbies
        public async Task GetLobbies()
        {
            var lobbies = lobbiesService.GetLobbies();
            var lobbyDtos = new List<LobbyDto>();

            foreach (var lobby in lobbies)
            {
                lobbyDtos.Add(Mapper.MapLobbyToLobbyDto(lobby));
            }

            await Clients.Caller.SendAsync("GetLobbies", lobbyDtos);
        }

        public async Task NewLobby(string lobbyName)
        {
            var lobby = lobbiesService.AddLobby(lobbyName);
            var player = lobbiesService.AddPlayerToLobby(lobby.Id, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);

            var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
            await Clients.Caller.SendAsync("LobbyCreatedByCurrentPlayer", lobbyDto);
            await Clients.All.SendAsync("LobbyCreated", lobbyDto);
        }

        public async Task JoinLobby(string lobbyId)
        {
            var lobbyName = lobbiesService.GetLobbyName(lobbyId);
            var player = lobbiesService.AddPlayerToLobby(lobbyId, Context.ConnectionId);
            var lobby = lobbiesService.GetPlayerLobby(Context.ConnectionId);

            await Clients.Group(lobbyName).SendAsync("PlayerJoinedLobby", player);

            var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
            await Clients.Caller.SendAsync("CurrentPlayerJoinedLobby", lobbyDto);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);
        }

        public async Task LeaveLobby(string lobbyId)
        {
            var lobby = lobbiesService.GetLobby(lobbyId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobby.Name);
            var player = lobbiesService.RemovePlayerFromLobby(lobbyId, Context.ConnectionId);

            if (lobby.Players.Count == 0)
            {
                lobbiesService.RemoveLobby(lobby.Id);

                var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
                await Clients.All.SendAsync("LobbyRemoved", lobbyDto);
            }
            else
            {
                await Clients.Group(lobbiesService.GetLobbyName(lobbyId)).SendAsync("PlayerLeftLobby", player);
            }

            await Clients.Caller.SendAsync("CurrentPlayerLeftLobby");
        }
        #endregion

        #region Chat
        public async Task NewMessage(Message msg, string lobbyId)
        {
            this.gameService.AddMessage(msg, lobbyId);
            await Clients.Group(lobbiesService.GetLobbyName(lobbyId)).SendAsync("MessageReceived", msg);   
        }
        #endregion
 
        public async Task StartGame(string lobbyId)
        {
            var startedLobby = gameService.StartGame(lobbyId);
            await Clients.All.SendAsync("GameStarted", startedLobby);
        }

        public async Task StopGame(string lobbyId)
        {
            var stoppedLobby = gameService.StartGame(lobbyId);
            await Clients.All.SendAsync("GameStarted", stoppedLobby);
        }

        #region Drawing
        public async Task NewSegmentPoint(Point point, string lobbyId)
        {
            gameService.AddSegmentPoint(point, lobbyId);
            await Clients.Group(lobbiesService.GetLobbyName(lobbyId)).SendAsync("SegmentPointReceived", point);
        }
        #endregion
    }
}
