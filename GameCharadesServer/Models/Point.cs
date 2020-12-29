using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class Point
    {
        public string ClientUniqueId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public bool IsFirstPointInSegment { get; set; }

        public bool IsLastPointInSegment { get; set; }

    }
}
