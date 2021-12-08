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
    public static class Day7
    {
        public static void Part1(string input)
        {
            var splitInput = input.Split(",");
            int[] crabPositions = new int[splitInput.Length];
            
            Console.WriteLine($"there are {crabPositions.Length} crabs");

            var minPosition = int.MaxValue;
            var maxPosition = 0;
            for (int i=0; i<splitInput.Length; i++)
            {
                int val = int.Parse(splitInput[i]);
                crabPositions[i] = val;
                if (val < minPosition)
                {
                    minPosition = val;
                }
                if (val > maxPosition)
                {
                    maxPosition = val;
                }
            }

            Console.WriteLine($"the max crab position is {maxPosition}");
            Console.WriteLine($"the min crab position is {minPosition}");

            var cheapestPosition = int.MaxValue;
            var cheapestCost = int.MaxValue;

            for (int i=minPosition; i<=maxPosition; i++)//; position in positions)
            {
                int cost = 0;
                foreach (int crab in crabPositions)
                {
                    cost += Math.Abs(crab - i);
                }
                if (cost < cheapestCost)
                {
                    cheapestCost = cost;
                    cheapestPosition = i;
                }
                Console.WriteLine($"it costs {cost} fuel to send the crabs to position {i}");
            }
            Console.WriteLine($"sending the crabs to position {cheapestPosition} costs {cheapestCost}");
        }
    }
}