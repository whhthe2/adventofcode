using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal static class Day03
    {
        static int lineLength;
        public static void Solve(string input)
        {

            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var lineCount = lines.Length;
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
                char b = getMostCommonValueAtPosition(position, arr, true);
                gammaRaw[position] = b;
                if (b == '0')
                {
                    
                    epsilonRaw[position] = '1';
                }
                else if (b == '1')
                {
                    epsilonRaw[position] = '0';
                }
            }

            var gamma = $"{new string(gammaRaw)}";
            var epsilon = $"{new string(epsilonRaw)}";
            Console.WriteLine($"gamma is {gamma}"); 
            Console.WriteLine($"epsilon is {epsilon}"); 
            var gammaRate = Convert.ToInt16(gamma, 2);
            var epsilonRate = Convert.ToInt16(epsilon, 2);
            Console.WriteLine($"p1 solution: {gammaRate*epsilonRate}");

            var oxygen = GetFilteredResult(arr, true);
            var oxygenRating = Convert.ToInt16(oxygen, 2);
            Console.WriteLine($"oxygen is {oxygen}"); 
            Console.WriteLine($"in decimal: {oxygenRating}");

            var c02 = GetFilteredResult(arr, false);
            var c02Rating = Convert.ToInt16(c02, 2);
            Console.WriteLine($"c02 is {c02}");
            Console.WriteLine($"in decimal: {c02Rating}");

            Console.WriteLine($"the total life support rating is: {oxygenRating * c02Rating}");
        }

        private static char getMostCommonValueAtPosition(int position, char[,] array, bool most)
        {
            var zeroCount = 0;
            var oneCount = 0;
            var lineCt = array.GetLength(1);
            for (int lineNo = 0; lineNo < lineCt; lineNo++)
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
                if (oneCount > zeroCount) { return '1'; }
                return '1';
            }
            else
            {
                if (zeroCount > oneCount) { return '1'; }
                if (oneCount > zeroCount) { return '0'; }
                return '0';
            }
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

        private static string GetFilteredResult(char[,] sourceArr, bool most)
        {
            char[,] workingArr = (char[,])sourceArr.Clone();
            var rawResult = new char[lineLength];
            for (int i=0; i<lineLength; i++)
            {
                //Most qualified value, based on the `most` param.
                //Either "most common" or "least common" (when false).
                char c = getMostCommonValueAtPosition(i, workingArr, most);;

                workingArr = keepMatchingValuesAtPosition(c, i, workingArr);
                
                //visualize
                var cursorPos = Console.GetCursorPosition();
                for (int y=0; y<workingArr.GetLength(1);y++)
                {
                    for (int x=0; x<workingArr.GetLength(0); x++)
                    {
                        Console.Write(workingArr[x,y]);
                    }
                    Console.SetCursorPosition(cursorPos.Left, cursorPos.Top);
                }
                
                if (workingArr.GetLength(1) == 1)
                {
                    Console.WriteLine();
                    for (int j=0; j<lineLength; j++)
                    {
                        rawResult[j] = workingArr[j, 0];
                    }
                    return new string(rawResult);
                }
            }
            throw new Exception("single result not found");
        }
    }
}