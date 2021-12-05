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
    public struct Coord
    {
        public int x;
        public int y;
        public Coord (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Line
    {
        public Coord start;
        public Coord end;
        public bool isStraight => start.x == end.x || start.y == end.y;

        //this returns chessboard distance, a.k.a. Chebyshev distance, 
        //NOT euclidean distance!
        public int Length 
        {
            get
            {
                return Math.Max(
                    Math.Abs(end.x - start.x), 
                    Math.Abs(end.y - start.y)
                );
            }
        }

        public Line(Coord a, Coord b)
        {
            start = a;
            end = b;
        }

        public Coord[] GetCoords()
        {
            //distances
            int xDist = end.x - start.x;
            int yDist = end.y - start.y;
            int xDir = xDist > 0 ? 1 : -1;
            int yDir = yDist > 0 ? 1 : -1;

            List<Coord> coords = new List<Coord>();
            for (int d=0; d<=Length; d++)
            {
                var x = start.x;
                var y = start.y;
                if (xDist != 0)
                {
                    x += d*xDir;
                }
                if (yDist != 0)
                {
                    y += d*yDir;
                }
                coords.Add(new Coord(x, y));
            }
            return coords.ToArray();
        }
    }

    internal static class Day5
    {
        public static void Part1(string input, int floorSize)
        {
            int[,] seafloor = new int[floorSize,floorSize];

            var inputLines = input.Split("\n");

            foreach (string line in inputLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                var splitLine = line.Split(" -> ");
                string startString = splitLine[0];
                string endString = splitLine[1];

                var splitStart = startString.Split(",");
                var splitEnd = endString.Split(",");
                Coord start = new Coord()
                {
                    x = int.Parse(splitStart[0]),
                    y = int.Parse(splitStart[1])
                };
                Coord end = new Coord()
                {
                    x = int.Parse(splitEnd[0]),
                    y = int.Parse(splitEnd[1]),
                };

                Line ventLine = new Line(start, end);
                foreach (var c in ventLine.GetCoords())
                {
                    seafloor[c.x,c.y] += 1;
                }
            }

            int overlapCount = 0;
            int consoleWidth = Console.BufferWidth;
            for (int y=0; y<floorSize; y++)
            {
                for (int x=0; x<floorSize; x++) 
                {
                    //visualize the data up until the point where the line would wrap
                    if (x < consoleWidth)
                    {
                        string val = seafloor[x,y] == 0 ? "." : seafloor[x,y].ToString();
                        Console.Write(val);
                    }
                    if ( seafloor[x,y] >= 2 )
                    {
                        overlapCount++;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(overlapCount.ToString());
        }
    }
}