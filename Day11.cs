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
            //Console.WriteLine(input);

            var inputLines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var xSize = inputLines[0].Length;
            var ySize = inputLines.Length;

            grid = new Dictionary<Coord,Octopus>(xSize*ySize);
            var gridBuildPos = Console.GetCursorPosition();
            for (int y=0; y<ySize; y++)
            {
                var row = inputLines[y].ToCharArray();
                for (int x=0; x<xSize; x++)
                {
                    var energy = int.Parse(row[x].ToString());
                    grid[new Coord(x,y)] = new Octopus(energy,x,y);
                    Visualize(xSize,ySize);
                }
                Console.SetCursorPosition(gridBuildPos.Left, gridBuildPos.Top);
            }

            int simLength = 100;
            while (simLength > 0)
            {
                simLength--;
                Tick(xSize,ySize);
                Visualize(xSize,ySize);
            }
            
        }

        public static void Tick(int xSize, int ySize)
        {
            for (int y=0; y<ySize; y++)
            {
                for (int x=0; x<xSize; x++) 
                {
                    Coord c;
                    var surge = grid[new Coord(x,y)].PowerUp(out c);
                    if (surge)
                    {
                        //gotta PowerUp my neighbors
                        //var nlist = GetNeighbors(c);
                    }
                    Console.Write(grid[new Coord(x,y)].Energy);
                }
                Console.WriteLine("");
            }
        }

        public static void Visualize(int xSize, int ySize)
        {
            for (int y=0; y<ySize; y++)
            {
                for (int x=0; x<xSize; x++) 
                {
                    Console.Write(grid[new Coord(x,y)].Energy);
                }
                Console.WriteLine("");
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
            Energy++;
            if (Energy > 9)
            {
                Energy = 0;
                c = position;
                return true;
            }
            c = new Coord(-1,-1);//an invalid coord;
            return false;
        }
    }
}