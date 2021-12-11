using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode2021
{
    internal static class Day11
    {
        public static void Solve(string input)
        {

            Console.WriteLine(input);
            Console.WriteLine("----------");
            var inputLines = input.Split("\n");
            var xSize = inputLines[0].Length;
            var ySize = inputLines.Length;

            var grid = new Dictionary<Coord,Octopus>();
            for (int y=0; y<ySize; y++)
            {
                var row = inputLines[y].ToCharArray();
                for (int x=0; x<xSize; x++)
                {
                    var energy = int.Parse(row[x].ToString());
                    grid[new Coord(x,y)] = new Octopus(energy,x,y);
                }
            }
            for (int y=0; y<ySize; y++)
            {
                for (int x=0; x<xSize; x++) 
                {
                    Console.Write(grid[new Coord(x,y)].Energy.ToString());
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