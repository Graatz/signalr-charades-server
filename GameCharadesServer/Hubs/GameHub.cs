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
        public GameHub(IGameService gameService)
        {
            this.gameService = gameService;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var lobby = gameService.GetPlayerLobby(Context.ConnectionId);

            // Remove player from lobby if he joined any
            if (lobby != null)
            {
                var player = gameService.RemovePlayerFromLobby(lobby.Id, Context.ConnectionId);

                if (lobby.Players.Count == 0)
                {
                    gameService.RemoveLobby(lobby.Id);
                    var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
                    Clients.All.SendAsync("LobbyRemoved", lobbyDto);
                }
                else
                {
                    Clients.Group(lobby.Name).SendAsync("PlayerLeftLobby", player);
                }

            }

            // Remove player
            gameService.RemovePlayer(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        #region Players
        public async Task CreatePlayer(string playerName)
        {
            var player = gameService.CreatePlayer(playerName, Context.ConnectionId);
            await Clients.Caller.SendAsync("PlayerCreated", player);
        }
        #endregion

        #region Lobbies
        public async Task GetLobbies()
        {
            var lobbies = gameService.GetLobbies();
            var lobbyDtos = new List<LobbyDto>();

            foreach (var lobby in lobbies)
            {
                lobbyDtos.Add(Mapper.MapLobbyToLobbyDto(lobby));
            }

            await Clients.Caller.SendAsync("GetLobbies", lobbyDtos);
        }

        public async Task NewLobby(string lobbyName)
        {
            var lobby = gameService.AddLobby(lobbyName);
            var player = gameService.AddPlayerToLobby(lobby.Id, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);

            var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
            await Clients.Caller.SendAsync("LobbyCreatedByCurrentPlayer", lobbyDto);
            await Clients.All.SendAsync("LobbyCreated", lobbyDto);
        }

        public async Task JoinLobby(string lobbyId)
        {
            var lobbyName = gameService.GetLobbyName(lobbyId);
            var player = gameService.AddPlayerToLobby(lobbyId, Context.ConnectionId);
            var lobby = gameService.GetPlayerLobby(Context.ConnectionId);

            await Clients.Group(lobbyName).SendAsync("PlayerJoinedLobby", player);

            var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
            await Clients.Caller.SendAsync("CurrentPlayerJoinedLobby", lobbyDto);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyName);
        }

        public async Task LeaveLobby(string lobbyId)
        {
            var lobby = gameService.GetLobby(lobbyId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobby.Name);
            var player = gameService.RemovePlayerFromLobby(lobbyId, Context.ConnectionId);

            if (lobby.Players.Count == 0)
            {
                gameService.RemoveLobby(lobby.Id);

                var lobbyDto = Mapper.MapLobbyToLobbyDto(lobby);
                await Clients.All.SendAsync("LobbyRemoved", lobbyDto);
            }
            else
            {
                await Clients.Group(gameService.GetLobbyName(lobbyId)).SendAsync("PlayerLeftLobby", player);
            }

            await Clients.Caller.SendAsync("CurrentPlayerLeftLobby");
        }
        #endregion

        #region Chat
        public async Task NewMessage(Message msg, string lobbyId)
        {
            this.gameService.AddMessage(msg, lobbyId);
            await Clients.Group(gameService.GetLobbyName(lobbyId)).SendAsync("MessageReceived", msg);   
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
            await Clients.Group(gameService.GetLobbyName(lobbyId)).SendAsync("SegmentPointReceived", point);
        }
        #endregion
    }
}
