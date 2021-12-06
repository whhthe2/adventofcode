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
    public static class Day6
    {
        public static void Part1(string input)
        {
            var school = Parse(input);
            var time = 0;
            var duration = 256;

            while (time < duration)
            {
                time++;

                List<Fish> newFishies = new List<Fish>();
                foreach (var fish in school)
                {
                    var newFish = fish.Update();
                    if (newFish != null)
                    {
                        newFishies.Add(newFish);
                    }
                }
                school.AddRange(newFishies);
            }
            Console.WriteLine($"There are {school.Count} fish after {duration} days");
        }

        private static List<Fish> Parse(string input)
        {
            var splitInput = input.Split(",");
            List<Fish> school = new List<Fish>();
            foreach (string value in splitInput)
            {
                int days = int.Parse(value);
                school.Add(new Fish(days));
            }
            return school;
        }
    }

    public class Fish
    {
        private const int juvenileGestationPeriod = 8;
        private const int adultGestationPeriod = 6;
        public int DaysUntilDue;

        public Fish(int daysUntilDue)
        {
            DaysUntilDue = daysUntilDue;
        }
        
        #nullable enable
        public Fish? Update()
        {
            DaysUntilDue--;
            if (DaysUntilDue < 0)
            {
                //reset to 6
                DaysUntilDue = adultGestationPeriod;
                //spawn
                return new Fish(juvenileGestationPeriod);
            }
            return null;
        }
        #nullable disable
    }
}