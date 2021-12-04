using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal static class Day3
    {
        static int lineCount;

        static int lineLength;

        //input here should be a filepath
        public static void Part1(string input)
        {

            var lines = File.ReadAllLines(input);
            lineCount = lines.Length;
            lineLength = lines[0].Trim().Length;
            char[,] arr = new char[lineLength, lineCount];
            
            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < lineLength; x++)
                {
                    arr[x,y] = lines[y][x];
                    //Console.Write(arr[x,y].ToString());
                }
                //Console.WriteLine();
            }
            
            char[] gammaRaw = new char[lineLength];
            char[] epsilonRaw = new char[lineLength];
            for (int position = 0; position < lineLength; position++)
            {
                char b = getMostCommonValueAtPosition(position, arr);
                gammaRaw[position] = b;
                if (b == '0')
                {
                    epsilonRaw[position] = '1';
                }
                else if (b == '1')
                {
                    epsilonRaw[position] = '0';
                }
                else
                {
                    //
                }
            }
            var gamma = $"{new string(gammaRaw)}";
            var epsilon = $"{new string(epsilonRaw)}";

            Console.WriteLine($"gamma is {gamma}"); //3797
            Console.WriteLine($"epsilon is {epsilon}"); //298

            var gammaRate = Convert.ToInt16(gamma, 2);
            var epsilonRate = Convert.ToInt16(epsilon, 2);
            
            var oxygen = GetFilteredResult(arr, true);
            var oxygenRating = Convert.ToInt16(oxygen, 2);
            Console.WriteLine($"oxygen is {oxygen}"); 
            Console.WriteLine($"in decimal: {oxygenRating}");

            var c02 = GetFilteredResult(arr, false);
            var c02Rating = Convert.ToInt16(oxygen, 2);
            Console.WriteLine($"c02 is {c02}");
            Console.WriteLine($"in decimal: {c02Rating}");

            Console.WriteLine($"the total life support rating is: {oxygenRating * c02Rating}");
        }

        private static char getMostCommonValueAtPosition(int position, char[,] array)
        {
            var zeroCount = 0;
            var oneCount = 0;
            for (int lineNo = 0; lineNo < lineCount; lineNo++)
            {
                var c = array[position, lineNo];
                if (c == '0')
                {
                    zeroCount++;
                }
                else if (c == '1')
                {
                    oneCount++;
                }
            }
            if (zeroCount > oneCount) { return '0'; }
            if (oneCount > zeroCount) { return '1'; }
            //throw new Exception("what a jerk puzzle designer!");
            return '9';
        }

        private static char getQualifiedCommonValueAtPosition(int position, char[,] array, bool most)
        {
            var zeroCount = 0;
            var oneCount = 0;
            for (int lineNo = 0; lineNo < array.GetLength(1); lineNo++)
            {
                var c = array[position, lineNo];
                if (c == '0')
                {
                    zeroCount++;
                }
                else if (c == '1')
                {
                    oneCount++;
                }
            }
            if (most)
            {
                if (zeroCount > oneCount) { return '0'; }
                if (oneCount >= zeroCount) { return '1'; }
            }
            else
            {
                if (zeroCount <= oneCount) { return '0'; }
                if (oneCount < zeroCount) { return '1'; }
            }
            
            //throw new Exception("what a jerk puzzle designer!");
            return '9';
        }

        private static char[,] keepMatchingValuesAtPosition(char keep, int position, char[,] array)
        {
            List<char[]> matched = new List<char[]>();
            var lCount = array.GetLength(1);
            for (int i=0; i<lCount; i++)
            {
                var val = array[position,i];
                if (val == keep)
                {
                    var value = Enumerable.Range(0, array.GetLength(0))
                                          .Select(x => array[x, i])
                                          .ToArray();
                    matched.Add(value);
                }
            }

            lCount = matched.Count;
            char[,] result = new char[lineLength, lCount];
            
            for (int y = 0; y < lCount; y++)
            {
                for (int x = 0; x < lineLength; x++)
                {
                    result[x,y] = matched[y][x];
                }
            }
            return result;
        }

        private static string GetFilteredResult(char[,] workingArr, bool most)
        {

            var oxygenRaw = new char[lineLength];
            for (int i=0; i<lineLength; i++)
            {
                char c = getQualifiedCommonValueAtPosition(i, workingArr, most);
                var filteredArr = keepMatchingValuesAtPosition(c, i, workingArr);
                workingArr = filteredArr;

                if (workingArr.GetLength(1) == 1)
                {
                    for (int j=0; j<lineLength; j++)
                    {
                        oxygenRaw[j] = workingArr[j, 0];
                    }
                    return new string(oxygenRaw);
                }
            }
            return null;
        }


    }
}