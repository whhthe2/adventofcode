using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal static class Day1
    {
        public static void Day1Part2(string input)
        {
            int[] measurements = prepareInput(input);
            
            List<int> groupedMeasures = new List<int>();
            for (int i=0; i<measurements.Length-2; i++)
            {
                int j = i+3;
                int groupSum = measurements[i..j].Sum();
                groupedMeasures.Add(groupSum);
            }
            int numIncreases = countIncreases(groupedMeasures.ToArray());
            Console.WriteLine($"These measurements increased {numIncreases} times.");
        }

        public static void Day1Part1(string input)
        {
            int[] preparedInput = prepareInput(input);
            int numIncreases = countIncreases(preparedInput);
            Console.WriteLine($"These measurements increased {numIncreases} times.");
        }

        
        private static int[] prepareInput(string input, char[] separator=null )
        {
            string[] splitInput = input.Split();
            int[] preparedInput = new int[splitInput.Length];
            for (int i=0; i<splitInput.Length; i++)
            {
                // convert to int
                int measurement;
                if (!int.TryParse(splitInput[i], System.Globalization.NumberStyles.Integer, null, out measurement))
                {
                    throw new Exception($"Cannot parse value as an integer: {splitInput[i]}");
                }
                preparedInput[i] = measurement;
            }
            return preparedInput;
        }

        private static int countIncreases(int[] measurements)
        {
            int increaseCounter = 0;
            int lastValue = -1;
            foreach (int measurement in measurements)
            {
                if (lastValue == -1)
                {
                    lastValue = measurement;
                    continue;
                }
                if (measurement > lastValue)
                {
                    increaseCounter++;
                }
                lastValue = measurement;
            }
            return increaseCounter;
        }
    }
}