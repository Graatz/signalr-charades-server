using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class LobbyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool InGame { get; set; }
        public List<Player> Players { get; set; }
    }
}
