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
    public static class Day06
    {
        private static bool printSimulation = false;

        public static long Solve(string input, int duration)
        {
            //key is the remaining gestation time of the fish
            //value is the number of fish at that state
            var school = new Dictionary<sbyte, long>{
                {0,0},{1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}
            };
            var startingPop = Parse(input);
            foreach (var val in startingPop)
            {
                school[val] += 1;
            }
            int cursorRow = Console.CursorTop+2;
            int cursorCol = 0;
            PrintSimulationStep(school, cursorRow, cursorCol);
            int time = 0;
            while (time < duration)
            {
                time++;
                var updatedSchool = new Dictionary<sbyte, long>{
                    {0,0},{1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}
                };
                foreach (var cohort in school)
                {
                    sbyte? newFish;
                    var result = ProcessFish(cohort.Key, out newFish);
                    updatedSchool[result] += cohort.Value;
                    if (newFish != null)
                    {
                        updatedSchool[newFish.Value] = cohort.Value;
                    }
                }
                school = updatedSchool;
                PrintSimulationStep(school, cursorRow, cursorCol);
            }
            long totalPop = 0;
            foreach (long count in school.Values)
            {
                totalPop += count;
            }
            return totalPop;
        }

        private static List<sbyte> Parse(string input)
        {
            var splitInput = input.Split(",");
            List<sbyte> school = new List<sbyte>();
            foreach (string value in splitInput)
            {
                var days = sbyte.Parse(value);
                school.Add(days);
            }
            return school;
        }

        public static sbyte ProcessFish(sbyte fish, out sbyte? newFish)
        {
            fish--;
            newFish = null;
            if (fish < 0)
            {
                //reset to 6
                fish = 6;
                //spawn
                newFish = 8;
            }
            return fish;
        }

        private static void PrintSimulationStep(Dictionary<sbyte, long> school, int cursorRow, int cursorCol )
        {
            if (!printSimulation)
            {
                return;
            }
            Console.SetCursorPosition(cursorRow, cursorCol);
            Console.WriteLine("\n");
            foreach (var cohort in school)
            {
                Console.WriteLine($"{cohort.Key}:{cohort.Value}");
            }
            Console.ReadKey(true);
        }
    }
}