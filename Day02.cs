using System;
using System.IO;

namespace adventofcode2021
{
    internal static class Day02
    {
        //input here should be a filepath
        public static void Solve(string input)
        {
            int position = 0;
            int aim = 0;
            int depth = 0;

            foreach (string line in File.ReadLines(input))
            {
                var splitLine = line.Split(" ");
                string command = splitLine[0];
                int distance = int.Parse(splitLine[1]);

                switch (command)
                {
                    case "forward": 
                        position += distance;
                        depth += (distance * aim);
                        break;
                    case "down":
                        aim += distance;break;
                    case "up":
                        aim -= distance;break;
                    default:
                        throw new Exception($"unrecognized command: '{command}'");
                }
            }

            Console.WriteLine($"position: {position}");
            Console.WriteLine($"depth: {depth}");
            Console.WriteLine($"answer: {position*depth}");
        }
        public static void Part1(string input)
        {
            int position = 0;
            int depth = 0;

            foreach (string line in File.ReadLines(input))
            {
                var splitLine = line.Split(" ");
                string command = splitLine[0];
                int distance = int.Parse(splitLine[1]);

                switch (command)
                {
                    case "forward": 
                        position += distance;break;
                    case "down":
                        depth += distance;break;
                    case "up":
                        depth -= distance;break;
                    default:
                        throw new Exception($"unrecognized command: '{command}'");
                }
            }

            Console.WriteLine($"position: {position}");
            Console.WriteLine($"depth: {depth}");
            Console.WriteLine($"answer: {position*depth}");
        }
    }
}