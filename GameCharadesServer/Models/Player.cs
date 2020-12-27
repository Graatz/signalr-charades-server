using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Avatar { get; set; }
        public string ConnectionId { get; set; }
    }
}
