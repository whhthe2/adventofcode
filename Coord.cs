using System.Collections.Generic;

namespace adventofcode2021
{   
    public struct Coord
    {
        public int x;
        public int y;

        public bool IsValid => x>-1 && y>-1;

        public Coord (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public List<Coord> GetNeighbors(bool includeDiagonals = true)
        {
            var neighbors = new List<Coord>(8);

            for (int y=-1; y<2; y++)
            {
                for (int x=-1; x<2; x++)
                {
                    if (y == 0 && x==0) {continue;} //skip 0,0-- that's the source location
                    if (!includeDiagonals && y != 0 && x!=0) {continue;} //skips diagonals
                    var neighbor = new Coord(this.x+x, this.y+y);    
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }
    }
}