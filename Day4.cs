using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal class BingoBoard
    {
        private const int size = 5;
        private int[][] board;
        public int[] this[int index]
        {
            get => board[index];
            set => board[index] = value;
        }
        public BingoBoard(string[] rows)
        {
            board = new int[size][size];
            int rowId = 0;
            foreach (var r in rows)
            {
                var cols = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length != size)
                {
                    throw new Exception($"Row does not have the correct number of elements: {r}");
                }
                int colId = 0;
                foreach (var c in cols)
                {
                    board[rowId][colId] = int.Parse(c);
                    colId++;
                }

                board[rowId] = r;
                rowId++;
            }
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
            }

            //organize boards
            foreach (var s in gameBoards)
            {
                Console.WriteLine(s);
            }
        }
        public static string ExtractValueSequence(string input, out string[] remainingValues)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new Exception ("cannot extract from empty input");
            }
            var splitInput = input.Split("\n");
            remainingValues = splitInput[1..^1];
            return splitInput[0];
        }
    }
}