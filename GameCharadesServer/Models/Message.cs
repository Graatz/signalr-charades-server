using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class Message
    {
        public string PlayerId { get; set; }
        public string LobbyId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
