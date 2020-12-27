using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class Point
    {
        public string ClientUniqueId { get; set; }

        public int CanvasWidth { get; set; }

        public int CanvasHeight { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public bool IsFirstPointInSegment { get; set; }

        public bool IsLastPointInSegment { get; set; }

    }
}
