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
        public static int Distance(Coord a, Coord b)
            => ( int )Math.Floor( Math.Sqrt( Math.Pow( b.x-a.x, 2 ) + Math.Pow( b.y-a.y, 2) ) );
    }

    public class Line
    {
        public Coord start;
        public Coord end;
        public bool isStraight => start.x == start.y || end.x == end.y;

        public Line(Coord a, Coord b)
        {
            start = a;
            end = b;
        }

        public static float Lerp(int a, int b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public Coord[] GetCoords()
        {
            int length = Coord.Distance(start, end);
            List<Coord> coords = new List<Coord>();
            for (float t=0; t<1; t+=(1f/length))
            {
                var x = (int)Math.Floor(Line.Lerp(start.x, end.x, t));
                var y = (int)Math.Floor(Line.Lerp(start.y, end.y, t));
                coords.Add(new Coord(x, y));
            }
            return coords.ToArray();
        }
    }

    internal static class Day5
    {
        public static void Part1(string input)
        {
            int floorSize = 999;
            int[,] seafloor = new int[floorSize,floorSize];

            var inputLines = input.Split("\n");

            foreach (string line in inputLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string sep = ":"; 
                var cleanLine = line.Replace(" -> ", sep);
                var splitLine = cleanLine.Split(sep);
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
                if (ventLine.isStraight)
                {
                    foreach (var c in ventLine.GetCoords())
                    {
                        seafloor[c.x,c.y] += 1;
                    }
                }
            }

            int overlapCount = 0;
            foreach (var x in seafloor)
            {
                if ( x >= 2 )
                {
                    overlapCount++;
                }
            }
            Console.WriteLine(overlapCount.ToString());

        }
    }
}