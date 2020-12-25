using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharadesServer.Models
{
    public class LineSegment
    {
        public List<Point> Points { get; set; }

        public LineSegment()
        {
            Points = new List<Point>();
        }
    }
}
