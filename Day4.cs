using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    public class BingoBoard
    {
        public BingoBoard()
        {
            
        }
        public string this[int index];
        {
            get;
            set;
        }
    }
    internal static class Day4
    {
        private static Queue<int> valueSequence;
        public static void Part1(string input)
        {
            string[] gameBoards;
            var rawValueSequence = ExtractValueSequence(input, out gameBoards );
            var splitValueSequence = rawValueSequence.Split(",");
            valueSequence = new Queue<int>();
            foreach (var s in splitValueSequence)
            {
                valueSequence.Append(int.Parse(s));
                Console.WriteLine(s);  
            }

            //organize boards
            
            foreach (var s in gameBoards)
            {
                
            }


        }

        public static string ExtractValueSequence(string input, out string[] remainingValues)
        {
            var splitInput = input.Split("\n");
            remainingValues = splitInput[1..^1];
            return splitInput[0];
        }
    }
}