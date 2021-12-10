using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/*
using NUnit.Framework;
from dotnet cli--
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Nunit3TestAdapter
dotnet add package NUnit
*/

namespace adventofcode2021
{   
    public static class Day9
    {

        static int[,] heightMap;

        public static void Part1(string input)
        {
            //Console.WriteLine(input);
            var splitInput = input.Split("\n");
            
            var dataWidth = splitInput[0].Length;
            var dataHeight = splitInput.Length;
            var mapWidth = dataWidth+2;
            var mapHeight = dataHeight+2;

            //fill a slightly oversized array with 9s
            heightMap = new int[mapWidth, mapHeight];
            for (int y=0; y<heightMap.GetLength(1); y++)
            {
                for (int x=0; x<heightMap.GetLength(0); x++)
                {
                    heightMap[x,y] = 9;
                }
            }

            for (int y=0; y<dataHeight; y++)
            {   
                var splitRow = splitInput[y].ToCharArray();      

                if (splitRow.Length < 1) {continue;}

                for (int x=0; x<dataWidth; x++)
                {
                    var val = (int) char.GetNumericValue(splitRow[x]);
                    heightMap[x+1,y+1] = val;
                }
            }

            int totalRisk = 0;

            List<Coord> lowPoints = new List<Coord>();

            for (int y=1; y<mapHeight-1; y++)
            {
                for (int x=1; x<mapWidth-1; x++)
                {
                    var neighbors = GetNeighbors(x, y);
                    var altitude = heightMap[x,y];
                    if (altitude == 9)
                    {
                        continue;
                    }
                    bool isLowPoint = true;
                    foreach (var c in neighbors)
                    {
                        if (heightMap[c.x,c.y] <= altitude)
                        {
                            isLowPoint = false;
                            break;
                        }
                    }
                    if (isLowPoint)
                    {
                        totalRisk += GetRiskLevel(altitude);
                        lowPoints.Add(new Coord(x,y));
                    }
                }
            }
            Console.WriteLine($"total risk: {totalRisk}");
            
            List<int> basinSizes = new List<int>();

            foreach (var coord in lowPoints)
            {
                basinSizes.Add(Flood(coord, IsFlooded));                
            }
            basinSizes.Sort();
            basinSizes.Reverse();
            var topBasins = basinSizes.Take(3);
            var solution = 1;
            foreach (var basin in topBasins)
            {
                solution *= basin;
            }
            Console.WriteLine(solution);
        }
        
        private static bool IsFlooded(Coord c)
        {
            if (heightMap[c.x,c.y] == 9)
            {
                return false;
            }
            return true;
        }

        public static int Flood(Coord coord, Func<Coord, bool> isFlooded )
        {
            HashSet<Coord> floodedCoords = new HashSet<Coord>();
            Queue<Coord> itinerary = new Queue<Coord>();
            itinerary.Enqueue(coord);
            
            while(itinerary.Count > 0)
            {
                var node = itinerary.Dequeue();

                if (isFlooded(node))
                {
                    if (floodedCoords.Add(node))
                    {
                        GetNeighbors(node).ForEach( x => itinerary.Enqueue( x ) );
                    }
                }
            } 
            return floodedCoords.Count;
        }

        public static List<Coord> GetNeighbors(int x, int y)
        {
            return GetNeighbors(new Coord(x,y));
        }
        public static List<Coord> GetNeighbors(Coord loc)
        {
            var neighbors = new List<Coord>(4);

            var w = heightMap.GetLength(0);
            var h = heightMap.GetLength(1);

            //try to get values from each neighbor.
            //add if found, catch and continue otherwise
            
            for (int y=-1; y<2; y++)
            {
                for (int x=-1; x<2; x++)
                {
                    if (y == 0 && x==0) {continue;} //skip 0,0-- that's the source location
                    if (y != 0 && x!=0) {continue;} //also skip the diagonals
                    int? value = null;
                    try
                    {
                        value = heightMap[loc.x+x, loc.y+y];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                    if (value.HasValue)
                    {
                        neighbors.Add(new Coord(loc.x+x, loc.y+y));
                    }
                    
                }
            }
            return neighbors;
        }

        public static int GetRiskLevel(int n)
        {
            return n+1;
        }
    }
}