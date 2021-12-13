using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode2021
{
    internal static class Day11
    {
        private static Dictionary<Coord,Octopus> grid;
        public static void Solve(string input)
        {
            var inputLines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var xSize = inputLines[0].Length;
            var ySize = inputLines.Length;
            var grid = new Dictionary<Coord,Octopus>();
            for (int y=0; y<ySize; y++)
            {
                var row = inputLines[y].ToCharArray();
                if (row.Length < xSize) { continue; }

                for (int x=0; x<xSize; x++)
                {
                    var energy = int.Parse(row[x].ToString());
                    grid[new Coord(x,y)] = new Octopus(energy,x,y);
                }    
            }
            var ticks = 100;
            while (ticks > 0)
            {
                ticks--;
                for (int y=0; y<ySize; y++){ for (int x=0; x<xSize; x++) {
                    grid[new Coord(x,y)].Energy++;
                }}

                for (int y=0; y<ySize; y++){ for (int x=0; x<xSize; x++) {
                    PowerUpWithNeighbors(new Coord(x,y), grid);
                }}

                for (int y=0; y<ySize; y++) { for (int x=0; x<xSize; x++) {
                    grid[new Coord(x,y)].Reset();
                }}
                Console.WriteLine($"score after one tick {Octopus.Score}");
            }

            Console.WriteLine($"final score: {Octopus.Score}");
        }


        public static void PowerUpWithNeighbors(Coord source, Dictionary<Coord, Octopus> grid)
        {
            Octopus srcOcto;
            if (!grid.TryGetValue(source, out srcOcto))
            {
                return;
            }
            Coord flash;
            
            if (srcOcto.PowerUp(out flash))
            {
                var validNeighbors = flash.GetNeighbors().Where( x => grid.ContainsKey(x)).ToList<Coord>();
                validNeighbors.ForEach( c => grid[c].Energy++ );
                validNeighbors.ForEach( c => PowerUpWithNeighbors(c, grid));
            }
        }
    }

    internal class Octopus
    {
        public int Energy;
        public bool HasFlashed;
        private Coord position;

        public Octopus(int startingEnergy, int x, int y)
        {
            Energy = startingEnergy;
            HasFlashed = false;
            position = new Coord(x, y);
        }

        public bool PowerUp(out Coord c)
        {
            if (Energy > 9 && !HasFlashed)
            {
                HasFlashed = true;
                c = position;
                return true;
            }
            c = new Coord(-1,-1);//an invalid coord;
            return false;
        }

        public static int Score = 0;
        public void Reset()
        {
            if (HasFlashed)
            {
                HasFlashed = false;
                Energy = 0;
                Score++;
            }
        }
    }
}