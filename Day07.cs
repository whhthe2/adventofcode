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
    public static class Day07
    {
        public static void Solve(string input)
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

            //fill out the cost of each step to facilitate the cumulative tally
            Dictionary<int, int> cumulativeCosts = new Dictionary<int, int>();
            cumulativeCosts.Add(0, 0);
            var cumulative = 0;
            for (int n=1; n<=maxPosition; n++)
            {
                cumulative += n;
                cumulativeCosts.Add(n, cumulative);
            }

            var cheapestPosition = int.MaxValue;
            var cheapestCost = int.MaxValue;

            for (int i=minPosition; i<=maxPosition; i++)            {
                int cost = 0;
                foreach (int crab in crabPositions)
                {
                    //distance
                    var d = Math.Abs(crab - i);

                    //p1 algo
                    //cost += d;

                    //p2 algo
                    cost += cumulativeCosts[d];
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